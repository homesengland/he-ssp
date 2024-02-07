using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class ProvideNumberOfGreenLightsCommandHandler : ProvideSiteDetailsBaseCommandHandler<ProvideNumberOfGreenLightsCommand>
{
    public ProvideNumberOfGreenLightsCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Provide(ProvideNumberOfGreenLightsCommand request, SiteEntity site)
    {
        var numberOfGreenLights = request.NumberOfGreenLights.IsProvided() ? new NumberOfGreenLights(request.NumberOfGreenLights!) : null;
        site.ProvideNumberOfGreenLights(numberOfGreenLights);
    }
}
