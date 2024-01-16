using HE.Investment.AHP.Domain.Application.Notifications;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Services.Notifications;

namespace HE.Investment.AHP.WWW.Notifications;

public class ApplicationStatusHasBeenChangedDisplayNotificationFactory : IDisplayNotificationFactory
{
    private readonly string _onHoldStatus = ApplicationStatus.OnHold.ToString();
    private readonly string _withdrawnStatus = ApplicationStatus.Withdrawn.ToString();

    public Type HandledNotificationType => typeof(ApplicationStatusHasBeenChangedNotification);

    public DisplayNotification Create(Notification notification)
    {
        var applicationStatus = notification.TemplateText($"<{ApplicationStatusHasBeenChangedNotification.ApplicationStatusParameterName}>");
        string? description;
        string? body;

        if (_onHoldStatus == applicationStatus)
        {
            description = "Your application has been put on hold.";
            body = "You’ll be able to reactivate and submit this application at any time.";
        }
        else if (_withdrawnStatus == applicationStatus)
        {
            description = "Your application has been withdrawn.";
            body = "You will no longer be able proceed with this application.";
        }
        else
        {
            throw new NotSupportedException($"Application status {applicationStatus} is not supported.");
        }

        return DisplayNotification.Success(
            description, body: body);
    }
}
