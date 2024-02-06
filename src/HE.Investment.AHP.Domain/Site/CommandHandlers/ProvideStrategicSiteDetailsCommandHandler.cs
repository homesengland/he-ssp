using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects.StrategicSite;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class ProvideStrategicSiteDetailsCommandHandler : ProvideSiteDetailsBaseCommandHandler<ProvideStrategicSiteDetailsCommand>
{
    public ProvideStrategicSiteDetailsCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Provide(ProvideStrategicSiteDetailsCommand request, SiteEntity site)
    {
        site.ProvideStrategicSiteDetails(StrategicSiteDetails.Create(request.IsStrategicSite, request.StrategicSiteName));
    }
}
