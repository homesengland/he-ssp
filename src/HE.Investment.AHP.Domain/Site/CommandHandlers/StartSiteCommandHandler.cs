using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.UserContext;
using HE.Investments.Account.Shared;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class StartSiteCommandHandler : ProvideSiteDetailsBaseCommandHandler<StartSiteCommand>
{
    private readonly IConsortiumUserContext _consortiumUserContext;

    public StartSiteCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext, IConsortiumUserContext consortiumUserContext)
        : base(siteRepository, accountUserContext)
    {
        _consortiumUserContext = consortiumUserContext;
    }

    protected override async void Provide(StartSiteCommand request, SiteEntity site)
    {
        var userAccount = await _consortiumUserContext.GetSelectedAccount();
        site.InitializeSitePartner(!userAccount.Consortium.HasNoConsortium, userAccount.SelectedOrganisation());
    }
}
