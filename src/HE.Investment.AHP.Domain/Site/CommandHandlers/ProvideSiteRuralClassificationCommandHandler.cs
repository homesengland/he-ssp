using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class ProvideSiteRuralClassificationCommandHandler : ProvideSiteDetailsBaseCommandHandler<ProvideSiteRuralClassificationCommand>
{
    public ProvideSiteRuralClassificationCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Provide(ProvideSiteRuralClassificationCommand request, SiteEntity site)
    {
        site.ProvideRuralClassification(new SiteRuralClassification(request.IsWithinRuralSettlement, request.IsRuralExceptionSite));
    }
}
