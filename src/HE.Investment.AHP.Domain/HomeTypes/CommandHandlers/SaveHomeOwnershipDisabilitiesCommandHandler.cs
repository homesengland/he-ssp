using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveHomeOwnershipDisabilitiesCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveHomeOwnershipDisabilitiesCommand>
{
    public SaveHomeOwnershipDisabilitiesCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IConsortiumUserContext accountUserContext,
        ILogger<SaveHomeOwnershipDisabilitiesCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => [HomeTypeSegmentType.TenureDetails];

    protected override IEnumerable<Action<SaveHomeOwnershipDisabilitiesCommand, IHomeTypeEntity>> SaveActions =>
    [
        (SaveHomeOwnershipDisabilitiesCommand _, IHomeTypeEntity homeType) => homeType.TenureDetails.ClearValuesForNewCalculation(),
        (request, homeType) => homeType.TenureDetails.ChangeMarketValue(request.MarketValue),
        (request, homeType) => homeType.TenureDetails.ChangeInitialSale(request.InitialSale),
        (_, homeType) => homeType.TenureDetails.ChangeExpectedFirstTranche(),
        (request, homeType) => homeType.TenureDetails.ChangeRentPerWeek(request.RentPerWeek),
        (_, homeType) => homeType.TenureDetails.ChangeRentAsPercentageOfTheUnsoldShare(),
    ];
}
