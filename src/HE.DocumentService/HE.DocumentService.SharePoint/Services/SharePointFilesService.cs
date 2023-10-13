using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HE.DocumentService.SharePoint.Configurartion;
using HE.DocumentService.SharePoint.Constants;
using HE.DocumentService.SharePoint.Exceptions;
using HE.DocumentService.SharePoint.Extensions;
using HE.DocumentService.SharePoint.Interfaces;
using HE.DocumentService.SharePoint.Models.File;
using HE.DocumentService.SharePoint.Models.Table;
using Microsoft.AspNetCore.Http;
using Microsoft.Graph;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.News.DataModel;
using Portable.Xaml.Markup;
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
                item => item["File_x0020_Size"],
                item => item["_ModerationComments"]),
                items => items.ListItemCollectionPosition);
        //_spContext.Load(listItems);
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

    public async Task UploadFile(FileUploadModel<IFormFile> item) => await UploadFile(_mapper.Map<FileUploadModel<FileData>>(item));

    public async Task UploadFile(FileUploadModel<FileData> item)
    {
        if (item.File.Data.Length == 0)
        {
            throw new SharepointException("The file is 0 bytes. Something went wrong, contact your system administrator.");
        }

        if (item.File.Data.Length > _spConfig.FileMaxSize)
        {
            throw new SharepointException($"Max file size is {Math.Round((double)_spConfig.FileMaxSize / 1024 / 1024, 2)}MB");
        }

        try
        {
            var folder = _sharePointFolderService.CreateFolderIfNotExist(item.ListTitle, item.FolderPath);

            var file = folder.Files.Add(new FileCreationInformation
            {
                ContentStream = new MemoryStream(item.File.Data),
                Url = item.File.Name,
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

    private string GetFolderPath(string listName, string folderPath)
    {
        return $"{new Uri(_spConfig.SiteUrl).AbsolutePath}/{listName}/{folderPath}";
    }
}
