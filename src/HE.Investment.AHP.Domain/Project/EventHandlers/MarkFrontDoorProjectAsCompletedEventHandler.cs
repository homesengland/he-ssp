using HE.Investment.AHP.Contract.Project.Events;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Repositories;

namespace HE.Investment.AHP.Domain.Project.EventHandlers;

public class MarkFrontDoorProjectAsCompletedEventHandler : IEventHandler<AhpProjectHasBeenCreatedEvent>
{
    private readonly IPrefillDataRepository _repository;

    public MarkFrontDoorProjectAsCompletedEventHandler(IPrefillDataRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(AhpProjectHasBeenCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        await _repository.MarkProjectAsUsed(new FrontDoorProjectId(domainEvent.FrontDoorProjectId.ToGuidAsString()), cancellationToken);
    }
}
