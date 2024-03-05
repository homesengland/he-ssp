using HE.DocumentService.SharePoint.Configuration;

namespace HE.DocumentService.Api.Configuration;

public interface IAppConfig
{
    public SharePointConfiguration SharePoint { get; set; }
}
