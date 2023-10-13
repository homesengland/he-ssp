using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HE.Investments.DocumentService.Configs;
using HE.Investments.DocumentService.Exceptions;
using HE.Investments.DocumentService.Models.File;
using HE.Investments.DocumentService.Models.Table;
using Microsoft.Extensions.Logging;

namespace HE.Investments.DocumentService.Services;

public interface IHttpDocumentService
{
    Task<TableResult<FileTableRow>> GetTableRowsAsync(FileTableFilter filter);

    Task UploadAsync(FileUploadModel item);

    Task DeleteAsync(string listAlias, string folderPath, string fileName);

    Task<FileData> DownloadAsync(string listAlias, string folderPath, string fileName);
}
