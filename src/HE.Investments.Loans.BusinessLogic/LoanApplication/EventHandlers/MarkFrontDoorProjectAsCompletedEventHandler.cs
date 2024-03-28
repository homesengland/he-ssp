using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Repositories;
using HE.Investments.Loans.Contract.Application.Events;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.EventHandlers;

public class MarkFrontDoorProjectAsCompletedEventHandler : IEventHandler<LoanApplicationHasBeenStartedEvent>
{
    private readonly IPrefillDataRepository _repository;

    public MarkFrontDoorProjectAsCompletedEventHandler(IPrefillDataRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(LoanApplicationHasBeenStartedEvent domainEvent, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(domainEvent.FrontDoorProjectId))
        {
            await _repository.MarkProjectAsUsed(new FrontDoorProjectId(domainEvent.FrontDoorProjectId!), cancellationToken);
        }
    }
}
