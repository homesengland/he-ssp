using Microsoft.SharePoint.Client;

namespace HE.UtilsService.SharePoint.Interfaces;

public interface ISharePointContext
{
    ClientContext Context { get; }
}
