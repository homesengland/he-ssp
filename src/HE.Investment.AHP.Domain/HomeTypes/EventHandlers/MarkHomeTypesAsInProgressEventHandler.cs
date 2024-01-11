using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Events;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Infrastructure.Events;

namespace HE.Investment.AHP.Domain.HomeTypes.EventHandlers;

public class MarkHomeTypesAsInProgressEventHandler :
    IEventHandler<HomeTypeHasBeenCreatedEvent>,
    IEventHandler<HomeTypeHasBeenUpdatedEvent>,
    IEventHandler<HomeTypeHasBeenRemovedEvent>
{
    private readonly IHomeTypeRepository _repository;

    private readonly IAccountUserContext _accountUserContext;

    public MarkHomeTypesAsInProgressEventHandler(IHomeTypeRepository repository, IAccountUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task Handle(HomeTypeHasBeenCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        await ChangeStatus(domainEvent.ApplicationId, cancellationToken);
    }

    public async Task Handle(HomeTypeHasBeenUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        await ChangeStatus(domainEvent.ApplicationId, cancellationToken);
    }

    public async Task Handle(HomeTypeHasBeenRemovedEvent domainEvent, CancellationToken cancellationToken)
    {
        await ChangeStatus(domainEvent.ApplicationId, cancellationToken);
    }

    private async Task ChangeStatus(AhpApplicationId applicationId, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var homeTypes = await _repository.GetByApplicationId(applicationId, account, HomeTypeSegmentTypes.None, cancellationToken);
        homeTypes.MarkAsInProgress();

        await _repository.Save(homeTypes, account.SelectedOrganisationId(), cancellationToken);
    }
}
