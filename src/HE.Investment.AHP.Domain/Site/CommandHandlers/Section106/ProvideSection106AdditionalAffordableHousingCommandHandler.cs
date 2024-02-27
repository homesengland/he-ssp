using HE.Investment.AHP.Contract.Site.Commands.Section106;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers.Section106;

public class ProvideSection106AdditionalAffordableHousingCommandHandler : ProvideSiteDetailsBaseCommandHandler<ProvideSection106AdditionalAffordableHousingCommand>
{
    public ProvideSection106AdditionalAffordableHousingCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Provide(ProvideSection106AdditionalAffordableHousingCommand request, SiteEntity site)
    {
        site.ProvideSection106(site.Section106.WithAdditionalAffordableHousing(request.AdditionalAffordableHousing));
    }
}
