using AutoMapper;
using HE.DocumentService.SharePoint.Configurartion;
using HE.DocumentService.SharePoint.Constants;
using HE.DocumentService.SharePoint.Exceptions;
using HE.DocumentService.SharePoint.Extensions;
using HE.DocumentService.SharePoint.Interfaces;
using HE.DocumentService.SharePoint.Models.File;
using HE.DocumentService.SharePoint.Models.Table;
using Microsoft.SharePoint.Client;
using File = Microsoft.SharePoint.Client.File;

namespace HE.DocumentService.SharePoint.Services;

public class SharePointFilesService : BaseService, ISharePointFilesService
{
    private readonly ISharePointFolderService _sharePointFolderService;

    public SharePointFilesService(ISharePointContext spContext,
        ISharePointConfiguration spConfig,
        IMapper mapper,
        ISharePointFolderService sharePointFolderService)
        : base(spContext, spConfig, mapper)
    {
        _sharePointFolderService = sharePointFolderService;
    }

    public async Task<TableResult<FileTableRow>> GetTableRows(FileTableFilter filter)
    {
        var take = 20;
        var documentsList = _spContext.Web.Lists.GetByTitle(filter.ListTitle);
        var camlQuery = new CamlQuery();
        camlQuery = new CamlQuery()
        {
            ViewXml = $@"<View Scope='RecursiveAll'>
                        <Query>
                            <Where>
                                <And>
                                    {GetCamlQueryForFolderPaths(filter.ListAlias, filter.FolderPaths)}
                                    <Eq>
                                        <FieldRef Name='FSObjType' />
                                        <Value Type='FSObjType'>0</Value>
                                    </Eq>
                                </And>
                            </Where>
                            <OrderBy><FieldRef Name='Modified' Ascending='false'/></OrderBy>
                        </Query>
                        <RowLimit>{take}</RowLimit>
                    </View>"
        };
        camlQuery.SetPostion(filter.PagingInfo);
        var listItems = documentsList.GetItems(camlQuery);
        _spContext.Load(listItems, items => items.Include(
                item => item["ID"],
                item => item["FileRef"],
                item => item["FileLeafRef"],
                item => item["FileDirRef"],
                item => item["Modified"],
                item => item["File_x0020_Size"],
                item => item["_ModerationComments"],
                item => item["Editor"]),
                items => items.ListItemCollectionPosition);
        //_spContext.Load(listItems);
        await _spContext.ExecuteQueryRetryAsync(RETRY_COUNT);

#pragma warning disable CA2000 // Dispose objects before losing scope
        var data = listItems.MapDataTable();
#pragma warning restore CA2000 // Dispose objects before losing scope
        var rows = _mapper.Map<List<FileTableRow>>(data.Rows);

        var chunkToRemove = $"{new Uri(_spConfig.SiteUrl).AbsolutePath}/{filter.ListAlias}/";
        rows.ForEach(row => row.FolderPath = row.FolderPath.Replace(chunkToRemove, ""));

        return new TableResult<FileTableRow>(rows, pagingInfo: listItems.ListItemCollectionPosition?.PagingInfo);
    }

    public async Task<Stream> DownloadFileStream(string listAlias, string folderPath, string fileName)
    {
        var folder = _spContext.Web.GetFolderByServerRelativeUrl(GetFolderPath(listAlias, folderPath));
        var file = await folder.GetFileAsync(fileName);
        var fileInfo = file.OpenBinaryStream();
        await _spContext.ExecuteQueryRetryAsync(RETRY_COUNT);

        return fileInfo.Value;
    }

    public async Task RemoveFile(string listAlias, string folderPath, string fileName)
    {
        var folder = _spContext.Web.GetFolderByServerRelativeUrl(GetFolderPath(listAlias, folderPath));
        var file = folder.GetFile(fileName);
        file.DeleteObject();
        await _spContext.ExecuteQueryRetryAsync(RETRY_COUNT);
    }

    public async Task UploadFile(FileUploadModel item)
    {
        using var fileContent = item.File.OpenReadStream();

        if (fileContent.Length == 0)
        {
            throw new SharepointException("The file is 0 bytes. Something went wrong, contact your system administrator.");
        }

        if (fileContent.Length > _spConfig.FileMaxSize)
        {
            throw new SharepointException($"Max file size is {Math.Round((double)_spConfig.FileMaxSize / 1024 / 1024, 2)}MB");
        }

        try
        {
            var folder = _sharePointFolderService.CreateFolderIfNotExist(item.ListTitle, item.FolderPath);

            var file = folder.Files.Add(new FileCreationInformation
            {
                ContentStream = fileContent,
                Url = item.File.FileName,
                Overwrite = item.Overwrite ?? false
            });

            var fileFields = file.ListItemAllFields;

            fileFields["_ModerationComments"] = item.Metadata;
            fileFields.Update();
            await _spContext.ExecuteQueryRetryAsync(RETRY_COUNT);
        }
        catch (ServerException ex)
        {
            if (ex.ServerErrorCode == SPErrorCodes.FileAlreadyExists)
            {
                throw new SharepointException($"File {item.File.Name} already exists");
            }
        }
    }

    public async Task UploadFile(string name, Stream stream)
    {
        var folder = _sharePointFolderService.CreateFolderIfNotExist("Loan Application", "0000136/external/more-information-about-organization");
        using var contentStream = new MemoryStream();
        var uploadFile = folder.Files.Add(new FileCreationInformation { ContentStream = contentStream, Url = name, Overwrite = true });

        // var fileFields = file.ListItemAllFields;
        //
        // fileFields["_ModerationComments"] = item.Metadata;
        // fileFields.Update();
        // await _spContext.ExecuteQueryRetryAsync(RETRY_COUNT);
        await UploadFileSlicePerSlice(uploadFile, _spContext, stream);
    }

    public void CreateFolders(string listTitle, List<string> folderPaths)
    {
        folderPaths.ForEach(folderPath => _sharePointFolderService.CreateFolderIfNotExist(listTitle, folderPath));
    }

    private string GetFolderPath(string listName, string folderPath)
    {
        return $"{new Uri(_spConfig.SiteUrl).AbsolutePath}/{listName}/{folderPath}";
    }

    private string GetCamlQueryForFolderPaths(string listAlias, List<string> folderPaths)
    {
        var result = string.Join("", folderPaths.Select(folderPath => $@"
                                        <Eq>
                                            <FieldRef Name='FileDirRef'/>
                                            <Value Type='Text'>{GetFolderPath(listAlias, folderPath)}</Value>
                                        </Eq>
                                    "));

        return folderPaths.Count > 1 ? $"<Or>{result}</Or>" : result;
    }

    public async Task UploadFileSlicePerSlice(
        File uploadFile,
        ClientContext ctx,
        Stream stream,
        int fileChunkSizeInMb = 10)
    {
        var uploadId = Guid.NewGuid();
        var blockSize = fileChunkSizeInMb * 1024 * 1024;

        // Get the information about the folder that will hold the file.
        var buffer = new byte[blockSize];
        var lastBuffer = Array.Empty<byte>();
        long fileOffset = 0;
        int bytesRead;
        var first = true;
        var last = false;

        // Read data from file system in blocks.
        while ((bytesRead = await Read(stream, buffer, blockSize)) > 0)
        {
            // You've reached the end of the file.
            if (bytesRead < buffer.Length)
            {
                last = true;
                // Copy to a new buffer that has the correct size.
                lastBuffer = new byte[bytesRead];
                Array.Copy(buffer, 0, lastBuffer, 0, bytesRead);
            }

            if (first)
            {
                using var firstStream = new MemoryStream(buffer);

                var bytesUploaded = uploadFile.StartUpload(uploadId, firstStream);
                await ctx.ExecuteQueryAsync();
                fileOffset = bytesUploaded.Value;

                first = false;
            }
            else
            {
                if (last)
                {
                    using var lastStream = new MemoryStream(lastBuffer);

                    uploadFile.FinishUpload(uploadId, fileOffset, lastStream);
                    await ctx.ExecuteQueryAsync();

                    return;
                }

                using var middleStream = new MemoryStream(buffer);

                var bytesUploaded = uploadFile.ContinueUpload(uploadId, fileOffset, middleStream);
                await ctx.ExecuteQueryAsync();

                fileOffset = bytesUploaded.Value;
            }
        }
    }

    private static async Task<int> Read(Stream br, byte[] buffer, int bufferSize)
    {
#pragma warning disable CA1835
        var bytesRead = await br.ReadAsync(buffer, 0, buffer.Length);
        var totalRead = bytesRead;

        while (bytesRead > 0 && totalRead < bufferSize)
        {
            bytesRead = await br.ReadAsync(buffer, bytesRead, buffer.Length - totalRead);
            totalRead += bytesRead;
        }

#pragma warning restore CA1835
        return totalRead;
    }
}
