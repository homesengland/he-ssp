using HE.Investments.Account.Shared;
using HE.Investments.FrontDoor.Contract.Site.Commands;
using HE.Investments.FrontDoor.Domain.Site.Repository;
using HE.Investments.FrontDoor.Domain.Site.ValueObjects;

namespace HE.Investments.FrontDoor.Domain.Site.CommandHandlers;

public class ProvidePlanningStatusCommandHandler : SiteBaseCommandHandler<ProvidePlanningStatusCommand>
{
    public ProvidePlanningStatusCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Perform(ProjectSiteEntity projectSite, ProvidePlanningStatusCommand request)
    {
        projectSite.ProvidePlanningStatus(new PlanningStatus(request.PlanningStatus));
    }
}
