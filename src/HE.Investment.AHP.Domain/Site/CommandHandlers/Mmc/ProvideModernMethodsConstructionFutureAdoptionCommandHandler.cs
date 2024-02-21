using HE.Investment.AHP.Contract.Site.Commands.Mmc;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects.Mmc;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers.Mmc;

public class ProvideModernMethodsConstructionFutureAdoptionCommandHandler : ProvideSiteDetailsBaseCommandHandler<ProvideModernMethodsConstructionFutureAdoptionCommand>
{
    public ProvideModernMethodsConstructionFutureAdoptionCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Provide(ProvideModernMethodsConstructionFutureAdoptionCommand request, SiteEntity site)
    {
        var operationResult = OperationResult.New();
        var plans = operationResult.AggregateNullable(() => ModernMethodsOfConstructionPlans.Create(request.Plans));
        var impact = operationResult.AggregateNullable(() => ModernMethodsOfConstructionExpectedImpact.Create(request.ExpectedImpact));
        var futureAdoption = operationResult.AggregateNullable(() => ModernMethodsOfConstructionFutureAdoption.Create(plans, impact));

        operationResult.CheckErrors();

        site.ProvideModernMethodsOfConstruction(SiteModernMethodsOfConstruction.Create(site.ModernMethodsOfConstruction, futureAdoption));
    }
}
