using HE.Investment.AHP.Domain.HomeTypes.Notifications;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investment.AHP.WWW.Notifications;

public class HomeTypeHasBeenCreatedDisplayNotificationFactory : IDisplayNotificationFactory
{
    public Type HandledNotificationType => typeof(HomeTypeHasBeenCreatedNotification);

    public DisplayNotification Create(Notification notification)
    {
        return DisplayNotification.Success(
            notification.TemplateText($"<{HomeTypeHasBeenCreatedNotification.HomeTypeNameParameterName}> has been added to your application"));
    }
}
