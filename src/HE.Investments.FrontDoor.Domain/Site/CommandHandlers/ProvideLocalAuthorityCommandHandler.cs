using HE.Investments.Account.Shared;
using HE.Investments.FrontDoor.Contract.Site.Commands;
using HE.Investments.FrontDoor.Domain.Site.Repository;

namespace HE.Investments.FrontDoor.Domain.Site.CommandHandlers;

public class ProvideLocalAuthorityCommandHandler : SiteBaseCommandHandler<ProvideLocalAuthorityCommand>
{
    public ProvideLocalAuthorityCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Perform(ProjectSiteEntity projectSite, ProvideLocalAuthorityCommand request)
    {
        projectSite.ProvideLocalAuthority(request.LocalAuthorityId);
    }
}
