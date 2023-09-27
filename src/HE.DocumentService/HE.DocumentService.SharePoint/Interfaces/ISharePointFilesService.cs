using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.DocumentService.SharePoint.Models.File;
using HE.DocumentService.SharePoint.Models.Table;

namespace HE.DocumentService.SharePoint.Interfaces;

public interface ISharePointFilesService
{
    Task<TableResult<FileTableRow>> GetTableRows(FileTableFilter filter);

    Task UploadFile(SharepointFileUploadModel item);

    Task<FileData> DownloadFile(string listAlias, string folderPath, string fileName);

    Task RemoveFile(string listAlias, string folderPath, string fileName);
}
