using HE.Investments.Common.Services.Notifications;

namespace HE.Investment.AHP.WWW.Notifications.Allocations;

public class ClaimHasBeenApprovedNotificationFactory : IDisplayNotificationFactory
{
    public string HandledNotificationType => OrganisationNotificationType.ClaimHasBeenApprovedNotification;

    public DisplayNotification Create(Notification notification)
    {
        var milestoneName = notification.Parameters["MilestoneName"];
        return DisplayNotification.Success($"Your {milestoneName} milestone claim has been approved.");
    }
}
