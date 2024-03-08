using HE.Investments.Account.Shared;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Notifications;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Contract.Application.Events;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.EventHandlers;

public class LoanApplicationSectionHasBeenCompletedAgainEventHandler : IEventHandler<LoanApplicationSectionHasBeenCompletedAgainEvent>
{
    private readonly INotificationPublisher _notificationPublisher;
    private readonly ILoanApplicationRepository _loanApplicationRepository;
    private readonly IAccountUserContext _loanUserContext;

    public LoanApplicationSectionHasBeenCompletedAgainEventHandler(
        INotificationPublisher notificationPublisher,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext)
    {
        _notificationPublisher = notificationPublisher;
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task Handle(LoanApplicationSectionHasBeenCompletedAgainEvent domainEvent, CancellationToken cancellationToken)
    {
        var loanApplication =
            await _loanApplicationRepository.GetLoanApplication(domainEvent.LoanApplicationId, await _loanUserContext.GetSelectedAccount(), cancellationToken);

        if (loanApplication.WasSubmitted())
        {
            await _notificationPublisher.Publish(new SectionCompletedAgainNotification());
        }
    }
}
