using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class ProvideTravellerPitchTypeDetailsCommandHandler : ProvideSiteDetailsBaseCommandHandler<ProvideTravellerPitchSiteTypeCommand>
{
    public ProvideTravellerPitchTypeDetailsCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Provide(ProvideTravellerPitchSiteTypeCommand request, SiteEntity site)
    {
        var siteUseDetails = site.SiteUseDetails.WithTravellerPitchSite(request.TravellerPitchSiteType);
        site.ProvideSiteUseDetails(siteUseDetails);
    }
}
