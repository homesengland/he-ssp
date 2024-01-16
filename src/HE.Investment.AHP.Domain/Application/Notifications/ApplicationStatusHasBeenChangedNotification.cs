using HE.Investments.Common.Contract;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investment.AHP.Domain.Application.Notifications;

public class ApplicationStatusHasBeenChangedNotification : Notification
{
    public const string ApplicationStatusParameterName = "ApplicationStatusParameterName";

    public ApplicationStatusHasBeenChangedNotification(ApplicationStatus applicationStatus)
        : base(
            nameof(ApplicationStatusHasBeenChangedNotification),
            new Dictionary<string, string> { { ApplicationStatusParameterName, applicationStatus.ToString() } })
    {
    }
}
