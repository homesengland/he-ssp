using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class CalculateRentToBuyQueryHandler : CalculateQueryHandlerBase<CalculateRentToBuyQuery>
{
    public CalculateRentToBuyQueryHandler(
        IHomeTypeRepository homeTypeRepository,
        IAccountUserContext accountUserContext,
        ILogger<CalculateRentToBuyQueryHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IEnumerable<Action<CalculateRentToBuyQuery, IHomeTypeEntity>> CalculateActions => new[]
    {
        (CalculateRentToBuyQuery request, IHomeTypeEntity homeType) => homeType.TenureDetails.ChangeMarketValue(request.MarketValue, true),
        (request, homeType) => homeType.TenureDetails.ChangeMarketRentPerWeek(request.MarketRentPerWeek, true),
        (request, homeType) => homeType.TenureDetails.ChangeRentPerWeek(request.RentPerWeek, isCalculation: true),
        (request, homeType) => homeType.TenureDetails.ChangeTargetRentExceedMarketRent(request.TargetRentExceedMarketRent, true),
        (_, homeType) => homeType.TenureDetails.ChangeRentAsPercentageOfMarketRent(),
    };

    protected override CalculationResult BuildCalculationResult(IHomeTypeEntity homeType)
    {
        return new CalculationResult(homeType.TenureDetails.ProspectiveRentAsPercentageOfMarketRent?.Value, null);
    }
}
