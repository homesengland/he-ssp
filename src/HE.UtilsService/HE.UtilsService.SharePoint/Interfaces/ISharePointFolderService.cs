using Microsoft.SharePoint.Client;

namespace HE.DocumentService.SharePoint.Interfaces;

public interface ISharePointFolderService
{
    Folder CreateFolderIfNotExist(string listTitle, string fullFolderPath);

    Folder EnsureAndGetTargetFolder(string folderPath);

    Folder EnsureAndGetTargetFolder(List list, List<string> folderPath);
}
