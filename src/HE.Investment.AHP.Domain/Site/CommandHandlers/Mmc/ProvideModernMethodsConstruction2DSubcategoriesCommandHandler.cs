using HE.Investment.AHP.Contract.Site.Commands.Mmc;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects.Mmc;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers.Mmc;

public class ProvideModernMethodsConstruction2DSubcategoriesCommandHandler : ProvideSiteDetailsBaseCommandHandler<ProvideModernMethodsConstruction2DSubcategoriesCommand>
{
    public ProvideModernMethodsConstruction2DSubcategoriesCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Provide(ProvideModernMethodsConstruction2DSubcategoriesCommand request, SiteEntity site)
    {
        site.ProvideModernMethodsOfConstruction(SiteModernMethodsOfConstruction.Create(site.ModernMethodsOfConstruction, request.ModernMethodsConstruction2DSubcategories));
    }
}
