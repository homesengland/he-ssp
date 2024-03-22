using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class CalculateAffordableRentQueryHandler : CalculateQueryHandlerBase<CalculateAffordableRentQuery>
{
    public CalculateAffordableRentQueryHandler(
        IHomeTypeRepository homeTypeRepository,
        IAccountUserContext accountUserContext,
        ILogger<CalculateAffordableRentQueryHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.TenureDetails };

    protected override IEnumerable<Action<CalculateAffordableRentQuery, IHomeTypeEntity>> CalculateActions => new[]
    {
        (CalculateAffordableRentQuery request, IHomeTypeEntity homeType) => homeType.TenureDetails.ChangeMarketValue(request.MarketValue, true),
        (request, homeType) => homeType.TenureDetails.ChangeMarketRentPerWeek(request.MarketRentPerWeek, true),
        (request, homeType) => homeType.TenureDetails.ChangeRentPerWeek(request.AffordableRentPerWeek, rentType: "Affordable Rent", true),
        (request, homeType) => homeType.TenureDetails.ChangeTargetRentExceedMarketRent(request.TargetRentExceedMarketRent, true),
        (_, homeType) => homeType.TenureDetails.ChangeRentAsPercentageOfMarketRent(),
    };

    protected override CalculationResult BuildCalculationResult(IHomeTypeEntity homeType)
    {
        return new CalculationResult(homeType.TenureDetails.ProspectiveRentAsPercentageOfMarketRent?.Value, null);
    }
}
