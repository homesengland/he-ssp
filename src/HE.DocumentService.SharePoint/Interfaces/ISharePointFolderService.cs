using Microsoft.SharePoint.Client;
using System.Collections.Generic;

namespace HE.DocumentService.SharePoint.Interfaces;

public interface ISharePointFolderService
{
    Folder CreateFolderIfNotExist(string listTitle, string fullFolderPath);

    Folder EnsureAndGetTargetFolder(string folderPath);

    Folder EnsureAndGetTargetFolder(List list, List<string> folderPath);

    Folder GetFolder(string path);
}
