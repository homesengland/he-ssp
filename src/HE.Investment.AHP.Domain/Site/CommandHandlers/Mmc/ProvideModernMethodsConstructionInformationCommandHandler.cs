using HE.Investment.AHP.Contract.Site.Commands.Mmc;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects.Mmc;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers.Mmc;

public class ProvideModernMethodsConstructionInformationCommandHandler : ProvideSiteDetailsBaseCommandHandler<
    ProvideModernMethodsConstructionInformationCommand>
{
    public ProvideModernMethodsConstructionInformationCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Provide(ProvideModernMethodsConstructionInformationCommand request, SiteEntity site)
    {
        var operationResult = OperationResult.New();
        var barriers = operationResult.AggregateNullable(() => ModernMethodsOfConstructionBarriers.Create(request.Barriers));
        var impact = operationResult.AggregateNullable(() => ModernMethodsOfConstructionImpact.Create(request.Impact));
        var information = operationResult.AggregateNullable(() => ModernMethodsOfConstructionInformation.Create(barriers, impact));

        operationResult.CheckErrors();

        site.ProvideModernMethodsOfConstruction(SiteModernMethodsOfConstruction.Create(site.ModernMethodsOfConstruction, information));
    }
}
