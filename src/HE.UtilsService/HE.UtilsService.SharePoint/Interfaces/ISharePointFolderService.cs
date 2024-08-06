using Microsoft.SharePoint.Client;

namespace HE.UtilsService.SharePoint.Interfaces;

public interface ISharePointFolderService
{
    Folder CreateFolderIfNotExist(string listTitle, string fullFolderPath);

    Folder EnsureAndGetTargetFolder(string folderPath);

    Folder EnsureAndGetTargetFolder(List list, List<string> folderPath);
}
