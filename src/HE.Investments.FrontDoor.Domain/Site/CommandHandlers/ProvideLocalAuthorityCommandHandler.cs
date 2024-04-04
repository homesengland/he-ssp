extern alias Org;

using HE.Investments.Account.Shared;
using HE.Investments.FrontDoor.Contract.Site.Commands;
using HE.Investments.FrontDoor.Domain.Site.Repository;
using SiteLocalAuthority = Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects.LocalAuthority;

namespace HE.Investments.FrontDoor.Domain.Site.CommandHandlers;

public class ProvideLocalAuthorityCommandHandler : SiteBaseCommandHandler<ProvideLocalAuthorityCommand>
{
    public ProvideLocalAuthorityCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Perform(ProjectSiteEntity projectSite, ProvideLocalAuthorityCommand request)
    {
        projectSite.ProvideLocalAuthority(SiteLocalAuthority.New(request.LocalAuthorityCode, request.LocalAuthorityName));
    }
}
