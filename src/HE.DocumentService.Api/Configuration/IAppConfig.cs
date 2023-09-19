using HE.DocumentService.SharePoint.Configurartion;

namespace HE.DocumentService.Api.Configuration;

public interface IAppConfig
{
    public SharePointConfiguration SharePoint { get; set; }
}