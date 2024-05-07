using HE.Investments.Account.Domain.Organisation.Notifications;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investments.Account.WWW.Notifications;

public class OrganisationAddedNotificationFactory : IDisplayNotificationFactory
{
    public string HandledNotificationType => nameof(OrganisationAddedNotification);

    public DisplayNotification Create(Notification notification)
    {
        return DisplayNotification.Success(notification.TemplateText($"<{OrganisationAddedNotification.OrganisationNameParameterName}> has been added."));
    }
}
