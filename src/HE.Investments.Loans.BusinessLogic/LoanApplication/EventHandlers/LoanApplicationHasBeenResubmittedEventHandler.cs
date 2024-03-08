using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Notifications;
using HE.Investments.Loans.Contract.Application.Events;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.EventHandlers;

public class LoanApplicationHasBeenResubmittedEventHandler : IEventHandler<LoanApplicationHasBeenResubmittedEvent>
{
    private readonly INotificationPublisher _notificationPublisher;

    public LoanApplicationHasBeenResubmittedEventHandler(INotificationPublisher notificationPublisher)
    {
        _notificationPublisher = notificationPublisher;
    }

    public async Task Handle(LoanApplicationHasBeenResubmittedEvent domainEvent, CancellationToken cancellationToken)
    {
        await _notificationPublisher.Publish(new LoanApplicationHasBeenResubmittedNotification());
    }
}
