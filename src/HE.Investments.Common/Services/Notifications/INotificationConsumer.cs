using HE.UtilsService.BannerNotification.Shared;

namespace HE.Investments.Common.Services.Notifications;

public interface INotificationConsumer
{
    DisplayNotification[] Pop(ApplicationArea? applicationArea);
}
