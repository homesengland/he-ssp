using HE.Investment.AHP.Contract.Site.Commands.Mmc;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects.Mmc;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers.Mmc;

public class ProvideModernMethodsConstruction3DSubcategoriesCommandHandler : ProvideSiteDetailsBaseCommandHandler<ProvideModernMethodsConstruction3DSubcategoriesCommand>
{
    public ProvideModernMethodsConstruction3DSubcategoriesCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Provide(ProvideModernMethodsConstruction3DSubcategoriesCommand request, SiteEntity site)
    {
        site.ProvideModernMethodsOfConstruction(SiteModernMethodsOfConstruction.Create(site.ModernMethodsOfConstruction, request.ModernMethodsConstruction3DSubcategories));
    }
}
