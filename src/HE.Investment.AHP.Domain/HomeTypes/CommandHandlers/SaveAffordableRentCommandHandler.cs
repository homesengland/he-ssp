using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveAffordableRentCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveAffordableRentCommand>
{
    public SaveAffordableRentCommandHandler(IHomeTypeRepository homeTypeRepository, ILogger<SaveAffordableRentCommandHandler> logger)
        : base(homeTypeRepository, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.TenureDetails };

    protected override IEnumerable<Action<SaveAffordableRentCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveAffordableRentCommand request, IHomeTypeEntity homeType) => homeType.TenureDetails.ChangeHomeMarketValue(request.HomeMarketValue),
        (request, homeType) => homeType.TenureDetails.ChangeHomeWeeklyRent(request.HomeWeeklyRent),
        (request, homeType) => homeType.TenureDetails.ChangeAffordableWeeklyRent(request.AffordableWeeklyRent),
        (request, homeType) => homeType.TenureDetails.ChangeTargetRentExceedMarketRent(request.TargetRentExceedMarketRent),
        (request, homeType) => homeType.TenureDetails.CalculateAffordableRentAsPercentageOfMarketRent(request.HomeWeeklyRent, request.AffordableWeeklyRent),
    };
}
