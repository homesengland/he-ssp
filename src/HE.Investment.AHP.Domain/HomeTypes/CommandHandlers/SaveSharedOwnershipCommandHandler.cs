using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveSharedOwnershipCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveSharedOwnershipCommand>
{
    public SaveSharedOwnershipCommandHandler(IHomeTypeRepository homeTypeRepository, ILogger<SaveSharedOwnershipCommandHandler> logger)
        : base(homeTypeRepository, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.TenureDetails };

    protected override IEnumerable<Action<SaveSharedOwnershipCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveSharedOwnershipCommand request, IHomeTypeEntity homeType) => homeType.TenureDetails.ChangeMarketValue(request.MarketValue),
        (request, homeType) => homeType.TenureDetails.ChangeInitialSale(request.InitialSale),
        (request, homeType) => homeType.TenureDetails.ChangeExpectedFirstTranche(request.MarketValue, request.InitialSale),
        (request, homeType) => homeType.TenureDetails.ChangeProspectiveRent(request.ProspectiveRent),
        (request, homeType) =>
            homeType.TenureDetails.ChangeProspectiveRentAsPercentageOfTheUnsoldShare(
                request.MarketValue,
                request.ProspectiveRent,
                request.InitialSale),
    };
}
