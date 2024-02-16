using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class ProvideSiteEnvironmentalImpactCommandHandler : ProvideSiteDetailsBaseCommandHandler<ProvideSiteEnvironmentalImpactCommand>
{
    public ProvideSiteEnvironmentalImpactCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Provide(ProvideSiteEnvironmentalImpactCommand request, SiteEntity site)
    {
        var environmentalImpact = request.EnvironmentalImpact.IsProvided() ? new EnvironmentalImpact(request.EnvironmentalImpact) : null;
        site.ProvideEnvironmentalImpact(environmentalImpact);
    }
}
