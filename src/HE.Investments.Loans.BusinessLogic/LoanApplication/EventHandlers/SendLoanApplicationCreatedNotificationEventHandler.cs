using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.Loans.Contract.Application.Events;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.EventHandlers;

public class SendLoanApplicationCreatedNotificationEventHandler : IEventHandler<LoanApplicationHasBeenStartedEvent>
{
    private readonly INotificationPublisher _notificationPublisher;

    public SendLoanApplicationCreatedNotificationEventHandler(INotificationPublisher notificationPublisher)
    {
        _notificationPublisher = notificationPublisher;
    }

    public async Task Handle(LoanApplicationHasBeenStartedEvent domainEvent, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(domainEvent.FrontDoorProjectId))
        {
            await _notificationPublisher.Publish(ApplicationType.Account, ToNotification(domainEvent));
        }
    }

    private static Notification ToNotification(LoanApplicationHasBeenStartedEvent domainEvent)
    {
        return new Notification(
            "LoanApplicationHasBeenCreatedNotification",
            new Dictionary<string, string> { { "ApplicationName", domainEvent.ApplicationName } });
    }
}
