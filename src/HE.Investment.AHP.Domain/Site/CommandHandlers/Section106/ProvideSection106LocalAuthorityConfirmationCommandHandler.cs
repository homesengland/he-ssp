using HE.Investment.AHP.Contract.Site.Commands.Section106;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers.Section106;

public class ProvideSection106LocalAuthorityConfirmationCommandHandler : ProvideSiteDetailsBaseCommandHandler<ProvideSection106LocalAuthorityConfirmationCommand>
{
    public ProvideSection106LocalAuthorityConfirmationCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Provide(ProvideSection106LocalAuthorityConfirmationCommand request, SiteEntity site)
    {
        site.ProvideSection106(site.Section106.WithLocalAuthorityConfirmation(request.LocalAuthorityConfirmation));
    }
}
