using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HE.DocumentService.SharePoint.Configurartion;
using HE.DocumentService.SharePoint.Extensions;
using HE.DocumentService.SharePoint.Interfaces;
using HE.DocumentService.SharePoint.Models.File;
using HE.DocumentService.SharePoint.Models.Table;
using Microsoft.SharePoint.Client;
using File = Microsoft.SharePoint.Client.File;

namespace HE.DocumentService.SharePoint.Services;

public class SharePointFilesService : BaseService, ISharePointFilesService
{
    private const string FilesList = "Key Files";

    private readonly ISharePointFolderService sharePointFolderService;

    public SharePointFilesService(ISharePointContext spContext,
        ISharePointConfiguration spConfig,
        IMapper mapper,
        ISharePointFolderService sharePointFolderService)
        : base(spContext, spConfig, mapper)
    {
        this.sharePointFolderService = sharePointFolderService;
    }

    public async Task<TableResult<FileTableRow>> GetTableRows(Guid contactId, FileTableFilter filter)
    {
        var take = 20;
        var documentsList = spContext.Web.Lists.GetByTitle(FilesList);
        var camlQuery = new CamlQuery();
        camlQuery = new CamlQuery()
        {
            ViewXml = $@"<View Scope='RecursiveAll'>
                        <Query>
                            <Where>
                                <Eq>
                                    <FieldRef Name='FileDirRef'/>
                                    <Value Type='Text'>{GetPath(contactId)}</Value>
                                </Eq>
                            </Where>
                            <OrderBy><FieldRef Name='Modified' Ascending='false'/></OrderBy>
                        </Query>
                        <RowLimit>{take}</RowLimit>
                    </View>"
        };
        camlQuery.SetPostion(filter.PagingInfo);
        var listItems = documentsList.GetItems(camlQuery);
        spContext.Load(listItems, items => items.Include(
                item => item["ID"],
                item => item["FileRef"],
                item => item["FileLeafRef"],
                item => item["Modified"],
                item => item["File_x0020_Size"]),
                items => items.ListItemCollectionPosition);
        //spContext.Load(listItems);
        await spContext.ExecuteQueryRetryAsync(RETRY_COUNT);

        var data = listItems.MapDataTable();
        var rows = mapper.Map<List<FileTableRow>>(data.Rows);

        return new TableResult<FileTableRow>(rows, pagingInfo: listItems.ListItemCollectionPosition?.PagingInfo);
    }

    public async Task<FileData> DownloadFile(Guid contactId, string fileName)
    {
        var folder = sharePointFolderService.GetFolder(GetPath(contactId));
        File file = folder.GetFile(fileName);
        var fileInfo = file.OpenBinaryStream();
        await spContext.ExecuteQueryRetryAsync(RETRY_COUNT);

        return new FileData(fileName, fileInfo.Value);
    }

    public async Task RemoveFile(Guid contactId, string fileName)
    {
        var folder = sharePointFolderService.GetFolder(GetPath(contactId));
        var file = folder.GetFile(fileName);
        file.DeleteObject();
        await spContext.ExecuteQueryRetryAsync(RETRY_COUNT);
    }

    public async Task UploadFile(Guid contactId, FileData fileData)
    {
        var folder = sharePointFolderService.CreateFolderIfNotExist(FilesList, contactId.ToString());

        if (fileData.Data.Length == 0)
            throw new Exception("The file is 0 bytes. Something went wrong, contact your system administrator.");

        if (fileData.Data.Length > spConfig.FileMaxSize)
            throw new Exception($"Max file size is {Math.Round((double)spConfig.FileMaxSize / 1024 / 1024, 2)}MB");

        var fci = new FileCreationInformation
        {
            ContentStream = new MemoryStream(fileData.Data),
            Url = fileData.Name,
            Overwrite = true
        };

        folder.Files.Add(fci);

        await spContext.ExecuteQueryRetryAsync(RETRY_COUNT);
    }

    private string GetPath(Guid contactId)
    {
        return $"{new Uri(spConfig.SiteUrl).AbsolutePath}/{FilesList}/{contactId}";
    }
}
