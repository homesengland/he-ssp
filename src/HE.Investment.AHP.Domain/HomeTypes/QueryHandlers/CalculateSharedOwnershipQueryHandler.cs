using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class CalculateSharedOwnershipQueryHandler : CalculateQueryHandlerBase<CalculateSharedOwnershipQuery>
{
    public CalculateSharedOwnershipQueryHandler(
        IHomeTypeRepository homeTypeRepository,
        IAccountUserContext accountUserContext,
        ILogger<CalculateSharedOwnershipQueryHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.TenureDetails };

    protected override IEnumerable<Action<CalculateSharedOwnershipQuery, IHomeTypeEntity>> CalculateActions => new[]
    {
        (CalculateSharedOwnershipQuery request, IHomeTypeEntity homeType) => homeType.TenureDetails.ChangeMarketValue(request.MarketValue, true),
        (request, homeType) => homeType.TenureDetails.ChangeInitialSale(request.InitialSale, true),
        (_, homeType) => homeType.TenureDetails.ChangeExpectedFirstTranche(),
        (request, homeType) => homeType.TenureDetails.ChangeRentPerWeek(request.ProspectiveRent, rentType: "Shared Ownership rent", isCalculation: true),
        (_, homeType) => homeType.TenureDetails.ChangeRentAsPercentageOfTheUnsoldShare(),
    };

    protected override CalculationResult BuildCalculationResult(IHomeTypeEntity homeType)
    {
        return new CalculationResult(homeType.TenureDetails.RentAsPercentageOfTheUnsoldShare?.Value, homeType.TenureDetails.ExpectedFirstTranche?.Value);
    }
}
