using HE.UtilsService.BannerNotification.Configuration;
using HE.UtilsService.SharePoint.Configuration;

namespace HE.UtilsService.Api.Configuration;

public class AppConfig : IAppConfig
{
    public SharePointConfiguration SharePoint { get; set; }

    public NotificationConfig Notification { get; set; }
}
