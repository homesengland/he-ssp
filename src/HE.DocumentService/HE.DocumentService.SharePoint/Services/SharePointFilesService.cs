using AutoMapper;
using HE.DocumentService.SharePoint.Configuration;
using HE.DocumentService.SharePoint.Constants;
using HE.DocumentService.SharePoint.Exceptions;
using HE.DocumentService.SharePoint.Extensions;
using HE.DocumentService.SharePoint.Interfaces;
using HE.DocumentService.SharePoint.Models.File;
using HE.DocumentService.SharePoint.Models.Table;
using Microsoft.SharePoint.Client;

namespace HE.DocumentService.SharePoint.Services;

public class SharePointFilesService : ISharePointFilesService
{
    private readonly ClientContext _spContext;

    private readonly ISharePointConfiguration _spConfig;

    private readonly IMapper _mapper;

    private readonly ISharePointFolderService _sharePointFolderService;

    public SharePointFilesService(
        ISharePointContext spContext,
        ISharePointConfiguration spConfig,
        IMapper mapper,
        ISharePointFolderService sharePointFolderService)
    {
        _spContext = spContext.Context;
        _spConfig = spConfig;
        _mapper = mapper;
        _sharePointFolderService = sharePointFolderService;
    }

    public async Task<TableResult<FileTableRow>> GetTableRows(FileTableFilter filter)
    {
        var take = 20;
        var documentsList = _spContext.Web.Lists.GetByTitle(filter.ListTitle);
        var camlQuery = new CamlQuery()
        {
            ViewXml = $@"<View Scope='RecursiveAll'>
                        <Query>
                            <Where>
                                <And>
                                    <And>
                                        <And>
                                            {GetCamlQueryForFolderPaths(filter.ListAlias, filter.FolderPaths)}
                                            <Eq>
                                                <FieldRef Name='FSObjType' />
                                                <Value Type='FSObjType'>0</Value>
                                            </Eq>
                                        </And>
                                        <IsNull>
                                            <FieldRef Name='_VirusInfo' />
                                        </IsNull>
                                    </And>
                                    <IsNotNull>
                                        <FieldRef Name='_ModerationComments' />
                                    </IsNotNull>
                                </And>
                            </Where>
                            <OrderBy><FieldRef Name='Modified' Ascending='false'/></OrderBy>
                        </Query>
                        <RowLimit>{take}</RowLimit>
                    </View>",
        };
        camlQuery.SetPosition(filter.PagingInfo);
        var listItems = documentsList.GetItems(camlQuery);
        _spContext.Load(
            listItems,
            items => items.Include(
                item => item["ID"],
                item => item["FileRef"],
                item => item["FileLeafRef"],
                item => item["FileDirRef"],
                item => item["Modified"],
                item => item["File_x0020_Size"],
                item => item["_ModerationComments"],
                item => item["Editor"]),
            items => items.ListItemCollectionPosition);
        await _spContext.ExecuteQueryRetryAsync(_spConfig.RetryCount);

        using var data = listItems.MapDataTable();
        var rows = _mapper.Map<List<FileTableRow>>(data.Rows);

        var chunkToRemove = $"{new Uri(_spConfig.SiteUrl).AbsolutePath}/{filter.ListAlias}/";
        rows.ForEach(row => row.FolderPath = row.FolderPath.Replace(chunkToRemove, string.Empty));

        return new TableResult<FileTableRow>(rows, pagingInfo: listItems.ListItemCollectionPosition?.PagingInfo, trimStringLength: 512);
    }

    public async Task<Stream> DownloadFileStream(string listAlias, string folderPath, string fileName)
    {
        var folder = _spContext.Web.GetFolderByServerRelativeUrl(GetFolderPath(listAlias, folderPath));
        var file = await folder.GetFileAsync(fileName);
        var fileInfo = file.OpenBinaryStream();
        await _spContext.ExecuteQueryRetryAsync(_spConfig.RetryCount);

        return fileInfo.Value;
    }

    public async Task RemoveFile(string listAlias, string folderPath, string fileName)
    {
        var folder = _spContext.Web.GetFolderByServerRelativeUrl(GetFolderPath(listAlias, folderPath));
        var file = await folder.GetFileAsync(fileName);
        file.DeleteObject();
        await _spContext.ExecuteQueryRetryAsync(_spConfig.RetryCount);
    }

    public async Task UploadFile(FileUploadModel item)
    {
        await using var fileContent = item.File.OpenReadStream();

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
                Overwrite = item.Overwrite ?? false,
            });

            var fileFields = file.ListItemAllFields;

            fileFields["_ModerationComments"] = item.Metadata;
            fileFields.Update();
            await _spContext.ExecuteQueryRetryAsync(_spConfig.RetryCount);
        }
        catch (ServerException ex)
        {
            if (ex.ServerErrorCode == SPErrorCodes.FileAlreadyExists)
            {
                throw new SharepointException($"File {item.File.Name} already exists");
            }
        }
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
        var result = string.Join(string.Empty, folderPaths.Select(folderPath => $@"
                                        <Eq>
                                            <FieldRef Name='FileDirRef'/>
                                            <Value Type='Text'>{GetFolderPath(listAlias, folderPath)}</Value>
                                        </Eq>
                                    "));

        return folderPaths.Count > 1 ? $"<Or>{result}</Or>" : result;
    }
}
