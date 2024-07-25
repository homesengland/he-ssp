using HE.Investments.Common.Services.Notifications;

namespace HE.Investments.AHP.Allocation.Contract.Claims.Notifications;

public class ClaimHasBeenSubmittedNotification : Notification
{
    public ClaimHasBeenSubmittedNotification(string milestoneName)
        : base(nameof(ClaimHasBeenSubmittedNotification), new Dictionary<string, string> { { "MilestoneName", milestoneName } })
    {
    }
}
