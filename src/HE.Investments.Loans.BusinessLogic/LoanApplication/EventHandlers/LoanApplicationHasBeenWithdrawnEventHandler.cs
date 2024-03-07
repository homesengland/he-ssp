using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Models.App;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Notifications;
using HE.Investments.Loans.Contract.Application.Events;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.EventHandlers;

public class LoanApplicationHasBeenWithdrawnEventHandler : IEventHandler<LoanApplicationHasBeenWithdrawnEvent>
{
    private readonly INotificationPublisher _notificationPublisher;

    private readonly IAppConfig _appConfig;

    public LoanApplicationHasBeenWithdrawnEventHandler(INotificationPublisher notificationPublisher, IAppConfig appConfig)
    {
        _notificationPublisher = notificationPublisher;
        _appConfig = appConfig;
    }

    public async Task Handle(LoanApplicationHasBeenWithdrawnEvent domainEvent, CancellationToken cancellationToken)
    {
        await _notificationPublisher.Publish(new ApplicationWithdrawSuccessfullyNotification(
            domainEvent.ApplicationName.Value,
            _appConfig.FundingSupportEmail ?? "funding support"));
    }
}
