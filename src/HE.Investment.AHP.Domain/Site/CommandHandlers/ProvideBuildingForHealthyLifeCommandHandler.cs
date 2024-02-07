using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class ProvideBuildingForHealthyLifeCommandHandler : ProvideSiteDetailsBaseCommandHandler<ProvideBuildingForHealthyLifeCommand>
{
    public ProvideBuildingForHealthyLifeCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Provide(ProvideBuildingForHealthyLifeCommand request, SiteEntity site)
    {
        site.ProvideBuildingForHealthyLife(request.BuildingForHealthyLife);
    }
}
