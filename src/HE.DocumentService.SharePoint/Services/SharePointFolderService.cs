using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HE.DocumentService.SharePoint.Configurartion;
using HE.DocumentService.SharePoint.Interfaces;
using Microsoft.SharePoint.Client;

namespace HE.DocumentService.SharePoint.Services;

public class SharePointFolderService : BaseService, ISharePointFolderService
{
    public SharePointFolderService(ISharePointContext spContext, ISharePointConfiguration spConfig, IMapper mapper)
        : base(spContext, spConfig, mapper)
    {
    }


    public Folder CreateFolderIfNotExist(string listTitle, string fullFolderPath)
    {
        if (string.IsNullOrEmpty(fullFolderPath))
            throw new Exception("Wrong fullFolderPath!");
        var list = spContext.Web.Lists.GetByTitle(listTitle);
        return CreateFolderInternal(list.RootFolder, fullFolderPath);
    }

    public Folder EnsureAndGetTargetFolder(string folderPath)
    {
        List<string> folderNames = folderPath.Trim('/').Split("/").ToList();
        List documents = spContext.Web.Lists.GetByTitle(folderNames[0]);
        folderNames.RemoveAt(0);

        return EnsureAndGetTargetFolder(documents, folderNames);
    }

    public Folder EnsureAndGetTargetFolder(List list, List<string> folderPath)
    {
        Folder returnFolder = list.RootFolder;
        return (folderPath != null && folderPath.Count > 0)
            ? EnsureAndGetTargetSubfolder(list, folderPath)
            : returnFolder;
    }

    public Folder GetFolder(string path)
    {
        spContext.Load(spContext.Web, l => l.ServerRelativeUrl);
        spContext.ExecuteQueryRetry(RETRY_COUNT);

        int index = path.IndexOf(spContext.Web.ServerRelativeUrl);
        if (index == -1)
            throw new Exception("Wrong path!");

        string folderPath = path.Substring(index, path.Length - index);
        var folder = spContext.Web.GetFolderByServerRelativeUrl(folderPath);
        return folder;
    }

    private Folder EnsureAndGetTargetSubfolder(List list, List<string> folderPath)
    {
        Web web = spContext.Web;
        Folder currentFolder = list.RootFolder;
        spContext.Load(web, t => t.Url);
        spContext.Load(currentFolder);
        spContext.ExecuteQueryRetry(RETRY_COUNT);

        foreach (string folderPointer in folderPath)
        {
            try
            {
                currentFolder = FindOrCreateFolder(list, currentFolder, folderPointer);
            }
            catch (ServerException ex)
            {
                // -2130245363 = SPErrorCodes.FolderAlreadyExists
                if (ex.ServerErrorCode != -2130245363)
                    throw;

                currentFolder = FindOrCreateFolder(list, currentFolder, folderPointer);
            }
        }

        return currentFolder;
    }

    private Folder FindOrCreateFolder(List list, Folder currentFolder, string folderPointer)
    {
        FolderCollection folders = currentFolder.Folders;
        spContext.Load(folders);
        spContext.ExecuteQueryRetry(RETRY_COUNT);

        foreach (Folder existingFolder in folders)
        {
            if (existingFolder.Name.Equals(folderPointer, StringComparison.InvariantCultureIgnoreCase))
            {
                return existingFolder;
            }
        }

        return CreateFolder(list, currentFolder, folderPointer);
    }

    private Folder CreateFolder(List list, Folder currentFolder, string folderPointer)
    {
        var itemCreationInfo = new ListItemCreationInformation
        {
            UnderlyingObjectType = FileSystemObjectType.Folder,
            LeafName = folderPointer,
            FolderUrl = currentFolder.ServerRelativeUrl
        };

        ListItem folderItemCreated = list.AddItem(itemCreationInfo);
        folderItemCreated.Update();

        spContext.Load(folderItemCreated, f => f.Folder);
        spContext.ExecuteQueryRetry(RETRY_COUNT);

        return folderItemCreated.Folder;
    }

    private Folder CreateFolderInternal(Folder parentFolder, string fullFolderPath)
    {
        var folderUrls = fullFolderPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        string folderUrl = folderUrls[0];
        var curFolder = parentFolder.Folders.Add(folderUrl);
        spContext.Load(curFolder);
        spContext.ExecuteQueryRetry(RETRY_COUNT);
        if (folderUrls.Length > 1)
        {
            var folderPath = string.Join("/", folderUrls, 1, folderUrls.Length - 1);
            return CreateFolderInternal(curFolder, folderPath);
        }
        return curFolder;
    }
}
