using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class ProvideSiteProcurementsCommandHandler : ProvideSiteDetailsBaseCommandHandler<ProvideSiteProcurementsCommand>
{
    public ProvideSiteProcurementsCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Provide(ProvideSiteProcurementsCommand request, SiteEntity site)
    {
        site.ProvideProcurement(new SiteProcurements(request.Procurements));
    }
}
