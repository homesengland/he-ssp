using HE.DocumentService.SharePoint.Configuration;
using HE.DocumentService.SharePoint.Constants;
using HE.DocumentService.SharePoint.Exceptions;
using HE.DocumentService.SharePoint.Interfaces;
using Microsoft.SharePoint.Client;

namespace HE.DocumentService.SharePoint.Services;

public class SharePointFolderService : ISharePointFolderService
{
    private readonly ClientContext _spContext;

    private readonly ISharePointConfiguration _spConfig;

    public SharePointFolderService(ISharePointContext spContext, ISharePointConfiguration spConfig)
    {
        _spConfig = spConfig;
        _spContext = spContext.Context;
    }

    public Folder CreateFolderIfNotExist(string listTitle, string fullFolderPath)
    {
        if (string.IsNullOrEmpty(fullFolderPath))
        {
            throw new SharepointException("Wrong fullFolderPath!");
        }

        var list = _spContext.Web.Lists.GetByTitle(listTitle);
        return CreateFolderInternal(list.RootFolder, fullFolderPath);
    }

    public Folder EnsureAndGetTargetFolder(string folderPath)
    {
        var folderNames = folderPath.Trim('/').Split("/").ToList();
        var documents = _spContext.Web.Lists.GetByTitle(folderNames[0]);
        folderNames.RemoveAt(0);

        return EnsureAndGetTargetFolder(documents, folderNames);
    }

    public Folder EnsureAndGetTargetFolder(List list, List<string> folderPath)
    {
        var returnFolder = list.RootFolder;
        return folderPath.Count > 0
            ? EnsureAndGetTargetSubfolder(list, folderPath)
            : returnFolder;
    }

    private Folder EnsureAndGetTargetSubfolder(List list, List<string> folderPath)
    {
        var web = _spContext.Web;
        var currentFolder = list.RootFolder;
        _spContext.Load(web, t => t.Url);
        _spContext.Load(currentFolder);
        _spContext.ExecuteQueryRetry(_spConfig.RetryCount);

        foreach (var folderPointer in folderPath)
        {
            try
            {
                currentFolder = FindOrCreateFolder(list, currentFolder, folderPointer);
            }
            catch (ServerException ex)
            {
                if (ex.ServerErrorCode != SPErrorCodes.FolderAlreadyExists)
                {
                    throw;
                }

                currentFolder = FindOrCreateFolder(list, currentFolder, folderPointer);
            }
        }

        return currentFolder;
    }

    private Folder FindOrCreateFolder(List list, Folder currentFolder, string folderPointer)
    {
        var folders = currentFolder.Folders;
        _spContext.Load(folders);
        _spContext.ExecuteQueryRetry(_spConfig.RetryCount);

        var existingFolder = folders.FirstOrDefault(x => x.Name.Equals(folderPointer, StringComparison.OrdinalIgnoreCase));

        if (existingFolder != null)
        {
            return existingFolder;
        }

        return CreateFolder(list, currentFolder, folderPointer);
    }

    private Folder CreateFolder(List list, Folder currentFolder, string folderPointer)
    {
        var itemCreationInfo = new ListItemCreationInformation
        {
            UnderlyingObjectType = FileSystemObjectType.Folder,
            LeafName = folderPointer,
            FolderUrl = currentFolder.ServerRelativeUrl,
        };

        var folderItemCreated = list.AddItem(itemCreationInfo);
        folderItemCreated.Update();

        _spContext.Load(folderItemCreated, f => f.Folder);
        _spContext.ExecuteQueryRetry(_spConfig.RetryCount);

        return folderItemCreated.Folder;
    }

    private Folder CreateFolderInternal(Folder parentFolder, string fullFolderPath)
    {
        var folderUrls = fullFolderPath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        var folderUrl = folderUrls[0];
        var curFolder = parentFolder.Folders.Add(folderUrl);
        _spContext.Load(curFolder);
        _spContext.ExecuteQueryRetry(_spConfig.RetryCount);
        if (folderUrls.Length > 1)
        {
            var folderPath = string.Join("/", folderUrls, 1, folderUrls.Length - 1);
            return CreateFolderInternal(curFolder, folderPath);
        }

        return curFolder;
    }
}
