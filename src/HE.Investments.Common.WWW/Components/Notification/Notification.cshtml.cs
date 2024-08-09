using HE.Investments.Common.Services.Notifications;
using HE.UtilsService.BannerNotification.Shared;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.Notification;

public class Notification : ViewComponent
{
    private readonly INotificationConsumer _notificationConsumer;

    public Notification(INotificationConsumer notificationConsumer)
    {
        _notificationConsumer = notificationConsumer;
    }

    public IViewComponentResult Invoke(
        DisplayNotification? displayNotification = null,
        bool shouldAddTopMargin = false,
        ApplicationArea? applicationArea = null)
    {
        var displayNotifications = _notificationConsumer.Pop(applicationArea);
        if (displayNotification != null)
        {
            displayNotifications = displayNotifications.Append(displayNotification).ToArray();
        }

        return View("Notification", (displayNotifications, shouldAddTopMargin));
    }
}
