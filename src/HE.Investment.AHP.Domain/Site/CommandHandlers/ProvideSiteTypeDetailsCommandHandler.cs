using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class ProvideSiteTypeDetailsCommandHandler : ProvideSiteDetailsBaseCommandHandler<ProvideSiteTypeDetailsCommand>
{
    public ProvideSiteTypeDetailsCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Provide(ProvideSiteTypeDetailsCommand request, SiteEntity site)
    {
        site.ProvideSiteTypeDetails(new SiteTypeDetails(request.SiteType, request.IsOnGreenBelt, request.IsRegenerationSite));
    }
}
