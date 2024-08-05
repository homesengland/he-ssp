using HE.UtilsService.SharePoint.Configuration;

namespace HE.UtilsService.Api.Configuration;

public interface IAppConfig
{
    public SharePointConfiguration SharePoint { get; set; }
}
