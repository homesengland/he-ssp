using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveRentToBuyCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveRentToBuyCommand>
{
    public SaveRentToBuyCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IAccountUserContext accountUserContext,
        ILogger<SaveRentToBuyCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.TenureDetails };

    protected override IEnumerable<Action<SaveRentToBuyCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveRentToBuyCommand _, IHomeTypeEntity homeType) => homeType.TenureDetails.ClearValuesForNewCalculation(),
        (request, homeType) => homeType.TenureDetails.ChangeMarketValue(request.MarketValue),
        (request, homeType) => homeType.TenureDetails.ChangeMarketRentPerWeek(request.MarketRentPerWeek),
        (request, homeType) => homeType.TenureDetails.ChangeRentPerWeek(request.RentPerWeek),
        (request, homeType) => homeType.TenureDetails.ChangeTargetRentExceedMarketRent(request.TargetRentExceedMarketRent),
        (_, homeType) => homeType.TenureDetails.ChangeRentAsPercentageOfMarketRent(),
    };
}
