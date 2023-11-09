using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.DocumentService.SharePoint.Models.File;
using HE.DocumentService.SharePoint.Models.Table;
using Microsoft.AspNetCore.Http;

namespace HE.DocumentService.SharePoint.Interfaces;

public interface ISharePointFilesService
{
    Task<TableResult<FileTableRow>> GetTableRows(FileTableFilter filter);

    Task UploadFile(FileUploadModel item);

    Task<FileData> DownloadFile(string listAlias, string folderPath, string fileName);

    Task RemoveFile(string listAlias, string folderPath, string fileName);

    void CreateFolders(string listTitle, List<string> folderPaths);
}
