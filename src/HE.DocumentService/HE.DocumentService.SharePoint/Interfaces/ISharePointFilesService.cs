using HE.DocumentService.SharePoint.Models.File;
using HE.DocumentService.SharePoint.Models.Table;

namespace HE.DocumentService.SharePoint.Interfaces;

public interface ISharePointFilesService
{
    Task<TableResult<FileTableRow>> GetTableRows(FileTableFilter filter);

    Task UploadFile(FileUploadModel item);

    Task<Stream> DownloadFileStream(string listAlias, string folderPath, string fileName);

    Task RemoveFile(string listAlias, string folderPath, string fileName);

    void CreateFolders(string listTitle, List<string> folderPaths);
}
