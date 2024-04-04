using HE.Investments.Account.Shared;
using HE.Investments.FrontDoor.Contract.Site.Commands;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Domain.Site.Repository;

namespace HE.Investments.FrontDoor.Domain.Site.CommandHandlers;

public class ProvideSiteHomesNumberCommandHandler : SiteBaseCommandHandler<ProvideSiteHomesNumberCommand>
{
    public ProvideSiteHomesNumberCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Perform(ProjectSiteEntity projectSite, ProvideSiteHomesNumberCommand request)
    {
        projectSite.ProvideHomesNumber(new HomesNumber(request.HouseNumber, "number of homes"));
    }
}
