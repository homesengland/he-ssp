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
    Task<TableResult<FileTableRow>> GetTableRows(Guid contactId, FileTableFilter filter);

    Task UploadFile(Guid contactId, FileData fileData);

    Task<FileData> DownloadFile(Guid contactId, string fileName);

    Task RemoveFile(Guid contactId, string fileName);
}
