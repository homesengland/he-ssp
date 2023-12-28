using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveProspectiveRentCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveProspectiveRentCommand>
{
    public SaveProspectiveRentCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IAccountUserContext accountUserContext,
        ILogger<SaveProspectiveRentCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.TenureDetails };

    protected override IEnumerable<Action<SaveProspectiveRentCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveProspectiveRentCommand request, IHomeTypeEntity homeType) => homeType.TenureDetails.ChangeMarketValue(request.MarketValue),
        (request, homeType) => homeType.TenureDetails.ChangeMarketRent(request.MarketRent),
        (request, homeType) => homeType.TenureDetails.ChangeProspectiveRent(request.ProspectiveRent),
        (request, homeType) => homeType.TenureDetails.ChangeTargetRentExceedMarketRent(request.TargetRentExceedMarketRent),
        (_, homeType) => homeType.TenureDetails.ChangeProspectiveRentAsPercentageOfMarketRent(),
    };
}
