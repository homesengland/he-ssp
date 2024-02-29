using HE.DocumentService.SharePoint.Configuration;

namespace HE.DocumentService.Api.Configuration;

public class AppConfig : IAppConfig
{
    public SharePointConfiguration SharePoint { get; set; }
}
