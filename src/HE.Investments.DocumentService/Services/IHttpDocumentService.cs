using HE.Investments.DocumentService.Models.File;
using HE.Investments.DocumentService.Models.Table;

namespace HE.Investments.DocumentService.Services;

public interface IHttpDocumentService
{
    Task<TableResult<FileTableRow>> GetTableRowsAsync(FileTableFilter filter);

    Task UploadAsync(FileUploadModel item);

    Task DeleteAsync(string listAlias, string folderPath, string fileName);

    Task<FileData> DownloadAsync(string listAlias, string folderPath, string fileName);

    Task CreateFoldersAsync(string listTitle, List<string> folderPaths);
}
