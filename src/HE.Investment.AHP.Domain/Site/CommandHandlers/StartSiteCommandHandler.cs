using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.UserContext;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class StartSiteCommandHandler : ProvideSiteDetailsBaseCommandHandler<StartSiteCommand>
{
    private readonly IAhpUserContext _ahpUserContext;

    public StartSiteCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext, IAhpUserContext ahpUserContext)
        : base(siteRepository, accountUserContext)
    {
        _ahpUserContext = ahpUserContext;
    }

    protected override async void Provide(StartSiteCommand request, SiteEntity site)
    {
        var userAccount = await _ahpUserContext.GetSelectedAccount();
        site.InitializeSitePartner(!userAccount.Consortium.HasNoConsortium, userAccount.SelectedOrganisation());
    }
}
