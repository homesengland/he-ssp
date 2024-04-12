using HE.Investments.Common.Services.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.Notification;

public class Notification : ViewComponent
{
    private readonly INotificationConsumer _notificationConsumer;

    public Notification(INotificationConsumer notificationConsumer)
    {
        _notificationConsumer = notificationConsumer;
    }

    public IViewComponentResult Invoke(DisplayNotification? displayNotification = null)
    {
        displayNotification ??= _notificationConsumer.Pop();
        return View("Notification", displayNotification);
    }
}
