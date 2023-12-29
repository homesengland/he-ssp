using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class CalculateProspectiveRentQueryHandler : CalculateQueryHandlerBase<CalculateProspectiveRentQuery>
{
    public CalculateProspectiveRentQueryHandler(
        IHomeTypeRepository homeTypeRepository,
        IAccountUserContext accountUserContext,
        ILogger<CalculateProspectiveRentQueryHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.TenureDetails };

    protected override IEnumerable<Action<CalculateProspectiveRentQuery, IHomeTypeEntity>> CalculateActions => new[]
    {
        (CalculateProspectiveRentQuery request, IHomeTypeEntity homeType) => homeType.TenureDetails.ChangeMarketValue(request.MarketValue),
        (request, homeType) => homeType.TenureDetails.ChangeMarketRent(request.MarketRent, true),
        (request, homeType) => homeType.TenureDetails.ChangeProspectiveRent(request.ProspectiveRent, true),
        (request, homeType) => homeType.TenureDetails.ChangeTargetRentExceedMarketRent(request.TargetRentExceedMarketRent, true),
        (_, homeType) => homeType.TenureDetails.ChangeProspectiveRentAsPercentageOfMarketRent(),
    };

    protected override CalculationResult BuildCalculationResult(IHomeTypeEntity homeType)
    {
        return new CalculationResult(homeType.TenureDetails.ProspectiveRentAsPercentageOfMarketRent?.Value, null);
    }
}
