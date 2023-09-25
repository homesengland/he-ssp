using HE.DocumentService.SharePoint.Configurartion;

namespace HE.DocumentService.Api.Configuration;

public class AppConfig : IAppConfig
{
    public SharePointConfiguration SharePoint { get; set; }
}
