using HE.Investments.AHP.Allocation.Contract.Claims.Notifications;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investment.AHP.WWW.Notifications.Allocations;

public class ClaimHasBeenSubmittedNotificationFactory : IDisplayNotificationFactory
{
    public string HandledNotificationType => nameof(ClaimHasBeenSubmittedNotification);

    public DisplayNotification Create(Notification notification)
    {
        var milestoneName = notification.Parameters["MilestoneName"];
        return DisplayNotification.Success(
            $"Your claim has been submitted against the {milestoneName} milestone.",
            body: "We will notify you after we have reviewed your claim.");
    }
}
