using HE.Investments.Account.Shared;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.FrontDoor.Contract.Project.Events;
using HE.Investments.FrontDoor.Domain.Site.Repository;

namespace HE.Investments.FrontDoor.Domain.Project.EventHandlers;

public class FrontDoorProjectSitesAreNotIdentifiedEventHandler : IEventHandler<FrontDoorProjectSitesAreNotIdentifiedEvent>
{
    private readonly IAccountUserContext _accountUserContext;

    private readonly ISiteRepository _siteRepository;

    private readonly IRemoveSiteRepository _removeSiteRepository;

    public FrontDoorProjectSitesAreNotIdentifiedEventHandler(
        IAccountUserContext accountUserContext,
        ISiteRepository siteRepository,
        IRemoveSiteRepository removeSiteRepository)
    {
        _accountUserContext = accountUserContext;
        _siteRepository = siteRepository;
        _removeSiteRepository = removeSiteRepository;
    }

    public async Task Handle(FrontDoorProjectSitesAreNotIdentifiedEvent domainEvent, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var projectSites = await _siteRepository.GetProjectSites(domainEvent.ProjectId, userAccount, cancellationToken);
        await projectSites.RemoveAllProjectSites(_removeSiteRepository, userAccount, cancellationToken);
    }
}
