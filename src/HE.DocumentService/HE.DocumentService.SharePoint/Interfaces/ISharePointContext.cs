using Microsoft.SharePoint.Client;

namespace HE.DocumentService.SharePoint.Interfaces;

public interface ISharePointContext
{
    ClientContext Context { get; }
}
