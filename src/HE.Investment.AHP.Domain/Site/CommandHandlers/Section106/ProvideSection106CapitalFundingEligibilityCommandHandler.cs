using HE.Investment.AHP.Contract.Site.Commands.Section106;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers.Section106;

public class ProvideSection106CapitalFundingEligibilityCommandHandler : ProvideSiteDetailsBaseCommandHandler<ProvideSection106CapitalFundingEligibilityCommand>
{
    public ProvideSection106CapitalFundingEligibilityCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Provide(ProvideSection106CapitalFundingEligibilityCommand request, SiteEntity site)
    {
        site.ProvideSection106(site.Section106.WithCapitalFundingEligibility(request.CapitalFundingEligibility));
    }
}
