using HE.Investment.AHP.Contract.Site.Commands.PlanningDetails;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects.Factories;
using HE.Investments.Account.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers.PlanningDetails;

public class ProvideSitePlanningStatusCommandHandler : ProvideSiteDetailsBaseCommandHandler<ProvideSitePlanningStatusCommand>
{
    public ProvideSitePlanningStatusCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext, ILogger<SiteBaseCommandHandler> logger)
        : base(siteRepository, accountUserContext, logger)
    {
    }

    protected override void Provide(ProvideSitePlanningStatusCommand request, SiteEntity site)
    {
        var planningDetails = PlanningDetailsFactory.Create(site.PlanningDetails, request.SitePlanningStatus);

        site.ProvidePlanningDetails(planningDetails);
    }
}
