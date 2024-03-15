using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Loans.Contract.Application.Events;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.EventHandlers;

public class MarkFrontDoorProjectAsCompletedEventHandler : IEventHandler<LoanApplicationHasBeenStartedEvent>
{
    public Task Handle(LoanApplicationHasBeenStartedEvent domainEvent, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(domainEvent.FrontDoorProjectId))
        {
            // TODO AB#93237: save in CRM that front door project was converted to Loan Application
        }

        return Task.CompletedTask;
    }
}
