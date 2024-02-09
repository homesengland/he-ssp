using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class ProvideSiteUseDetailsCommandHandler : ProvideSiteDetailsBaseCommandHandler<ProvideSiteUseDetailsCommand>
{
    public ProvideSiteUseDetailsCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Provide(ProvideSiteUseDetailsCommand request, SiteEntity site)
    {
        var siteUseDetails = site.SiteUseDetails.WithSiteUse(request.IsPartOfStreetFrontInfill, request.IsForTravellerPitchSite);
        site.ProvideSiteUseDetails(siteUseDetails);
    }
}
