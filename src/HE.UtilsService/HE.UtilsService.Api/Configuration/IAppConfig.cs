using HE.UtilsService.BannerNotification.Configuration;
using HE.UtilsService.SharePoint.Configuration;

namespace HE.UtilsService.Api.Configuration;

public interface IAppConfig
{
    public SharePointConfiguration SharePoint { get; set; }

    public NotificationConfig Notification { get; set; }
}
