using HE.Investments.Account.Shared;
using HE.Investments.FrontDoor.Contract.Site.Commands;
using HE.Investments.FrontDoor.Domain.Site.Repository;

namespace HE.Investments.FrontDoor.Domain.Site.CommandHandlers;

public class ProvideSiteHomesNumberCommandHandler : SiteBaseCommandHandler<ProvideHomesNumberCommand>
{
    public ProvideSiteHomesNumberCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }
}
