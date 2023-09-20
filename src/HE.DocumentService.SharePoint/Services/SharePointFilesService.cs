using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HE.DocumentService.SharePoint.Configurartion;
using HE.DocumentService.SharePoint.Exceptions;
using HE.DocumentService.SharePoint.Extensions;
using HE.DocumentService.SharePoint.Interfaces;
using HE.DocumentService.SharePoint.Models.File;
using HE.DocumentService.SharePoint.Models.Table;
using Microsoft.Graph;
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
                                    <Eq>
                                        <FieldRef Name='FileDirRef'/>
                                        <Value Type='Text'>{GetFolderPath(filter.ListAlias, filter.FolderPath)}</Value>
                                    </Eq>
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
                item => item["Modified"],
                item => item["File_x0020_Size"]),
                items => items.ListItemCollectionPosition);
        _spContext.Load(listItems);
        await _spContext.ExecuteQueryRetryAsync(RETRY_COUNT);

#pragma warning disable CA2000 // Dispose objects before losing scope
        var data = listItems.MapDataTable();
#pragma warning restore CA2000 // Dispose objects before losing scope
        var rows = _mapper.Map<List<FileTableRow>>(data.Rows);

        return new TableResult<FileTableRow>(rows, pagingInfo: listItems.ListItemCollectionPosition?.PagingInfo);
    }

    public async Task<FileData> DownloadFile(string listAlias, string folderPath, string fileName)
    {
        var folder = _spContext.Web.GetFolderByServerRelativeUrl(GetFolderPath(listAlias, folderPath));
        var file = folder.GetFile(fileName);
        var fileInfo = file.OpenBinaryStream();
        await _spContext.ExecuteQueryRetryAsync(RETRY_COUNT);

        return new FileData(fileName, fileInfo.Value);
    }

    public async Task RemoveFile(string listAlias, string folderPath, string fileName)
    {
        var folder = _spContext.Web.GetFolderByServerRelativeUrl(GetFolderPath(listAlias, folderPath));
        var file = folder.GetFile(fileName);
        file.DeleteObject();
        await _spContext.ExecuteQueryRetryAsync(RETRY_COUNT);
    }

    public async Task UploadFile(SharepointFileUploadModel item)
    {
        var folder = _sharePointFolderService.CreateFolderIfNotExist(item.ListTitle, item.FolderPath);

        using var ms = new MemoryStream();
        await item.File.CopyToAsync(ms);
        var bytes = ms.ToArray();

        if (bytes.Length == 0)
        {
            throw new SharepointException("The file is 0 bytes. Something went wrong, contact your system administrator.");
        }

        if (bytes.Length > _spConfig.FileMaxSize)
        {
            throw new SharepointException($"Max file size is {Math.Round((double)_spConfig.FileMaxSize / 1024 / 1024, 2)}MB");
        }

        var fci = new FileCreationInformation
        {
            ContentStream = new MemoryStream(bytes),
            Url = item.File.FileName,
            Overwrite = true
        };

        folder.Files.Add(fci);

        await _spContext.ExecuteQueryRetryAsync(RETRY_COUNT);
    }

    private string GetFolderPath(string listName, string folderPath)
    {
        return $"{new Uri(_spConfig.SiteUrl).AbsolutePath}/{listName}/{folderPath}";
    }
}
