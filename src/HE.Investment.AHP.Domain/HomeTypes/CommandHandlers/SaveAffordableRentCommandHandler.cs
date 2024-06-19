using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveAffordableRentCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveAffordableRentCommand>
{
    public SaveAffordableRentCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IConsortiumUserContext accountUserContext,
        ILogger<SaveAffordableRentCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => [HomeTypeSegmentType.TenureDetails];

    protected override IEnumerable<Action<SaveAffordableRentCommand, IHomeTypeEntity>> SaveActions =>
    [
        (SaveAffordableRentCommand _, IHomeTypeEntity homeType) => homeType.TenureDetails.ClearValuesForNewCalculation(),
        (request, homeType) => homeType.TenureDetails.ChangeMarketValue(request.MarketValue),
        (request, homeType) => homeType.TenureDetails.ChangeMarketRentPerWeek(request.MarketRentPerWeek),
        (request, homeType) => homeType.TenureDetails.ChangeRentPerWeek(request.AffordableRentPerWeek, rentType: "Affordable Rent"),
        (request, homeType) => homeType.TenureDetails.ChangeTargetRentExceedMarketRent(request.TargetRentExceedMarketRent),
        (_, homeType) => homeType.TenureDetails.ChangeRentAsPercentageOfMarketRent(),
    ];
}
