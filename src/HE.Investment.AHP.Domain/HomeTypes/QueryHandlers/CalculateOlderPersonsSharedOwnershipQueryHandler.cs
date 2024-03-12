using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class CalculateOlderPersonsSharedOwnershipQueryHandler : CalculateQueryHandlerBase<CalculateOlderPersonsSharedOwnershipQuery>
{
    public CalculateOlderPersonsSharedOwnershipQueryHandler(
        IHomeTypeRepository homeTypeRepository,
        IAccountUserContext accountUserContext,
        ILogger<CalculateOlderPersonsSharedOwnershipQueryHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.TenureDetails };

    protected override IEnumerable<Action<CalculateOlderPersonsSharedOwnershipQuery, IHomeTypeEntity>> CalculateActions => new[]
    {
        (CalculateOlderPersonsSharedOwnershipQuery request, IHomeTypeEntity homeType) => homeType.TenureDetails.ChangeMarketValue(request.MarketValue, true),
        (request, homeType) => homeType.TenureDetails.ChangeInitialSale(request.InitialSale, true),
        (_, homeType) => homeType.TenureDetails.ChangeExpectedFirstTranche(),
        (request, homeType) => homeType.TenureDetails.ChangeRentPerWeek(request.RentPerWeek, isCalculation: true),
        (_, homeType) => homeType.TenureDetails.ChangeRentAsPercentageOfTheUnsoldShare(),
    };

    protected override CalculationResult BuildCalculationResult(IHomeTypeEntity homeType)
    {
        return new CalculationResult(homeType.TenureDetails.RentAsPercentageOfTheUnsoldShare?.Value, homeType.TenureDetails.ExpectedFirstTranche?.Value);
    }
}
