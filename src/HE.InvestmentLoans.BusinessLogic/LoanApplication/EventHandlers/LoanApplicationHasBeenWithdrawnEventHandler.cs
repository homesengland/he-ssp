using HE.InvestmentLoans.Common.Contract.Services.Interfaces;
using HE.InvestmentLoans.Common.Models.App;
using HE.InvestmentLoans.Common.Utils.Constants.Notification;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.Application.Events;
using HE.Investments.Common.Infrastructure.Events;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.EventHandlers;

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
        var valuesToDisplay = new Dictionary<NotificationServiceKeys, string>
        {
            { NotificationServiceKeys.Name, domainEvent.ApplicationName.Value },
            { NotificationServiceKeys.Email, _appConfig.FundingSupportEmail ?? "funding support" },
        };

        await _notificationService.NotifySuccess(NotificationBodyType.WithdrawApplication, valuesToDisplay);
    }
}
