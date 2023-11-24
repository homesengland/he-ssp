using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Notifications;
using HE.Investments.Loans.Common.Models.App;
using HE.Investments.Loans.Contract.Application.Events;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.EventHandlers;

public class LoanApplicationHasBeenWithdrawnEventHandler : IEventHandler<LoanApplicationHasBeenWithdrawnEvent>
{
    private readonly INotificationService _notificationService;

    private readonly IAppConfig _appConfig;

    public LoanApplicationHasBeenWithdrawnEventHandler(INotificationService notificationService, IAppConfig appConfig)
    {
        _notificationService = notificationService;
        _appConfig = appConfig;
    }

    public async Task Handle(LoanApplicationHasBeenWithdrawnEvent domainEvent, CancellationToken cancellationToken)
    {
        await _notificationService.Publish(new ApplicationWithdrawSuccessfullyNotification(
            domainEvent.ApplicationName.Value,
            _appConfig.FundingSupportEmail ?? "funding support"));
    }
}
