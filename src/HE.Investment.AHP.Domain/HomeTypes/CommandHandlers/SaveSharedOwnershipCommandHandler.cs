using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveSharedOwnershipCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveSharedOwnershipCommand>
{
    public SaveSharedOwnershipCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IAccountUserContext accountUserContext,
        ILogger<SaveSharedOwnershipCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.TenureDetails };

    protected override IEnumerable<Action<SaveSharedOwnershipCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveSharedOwnershipCommand request, IHomeTypeEntity homeType) => homeType.TenureDetails.ChangeMarketValue(request.MarketValue),
        (request, homeType) => homeType.TenureDetails.ChangeInitialSale(request.InitialSale),
        (_, homeType) => homeType.TenureDetails.ChangeExpectedFirstTranche(),
        (request, homeType) => homeType.TenureDetails.ChangeProspectiveRent(request.ProspectiveRent),
        (_, homeType) => homeType.TenureDetails.ChangeProspectiveRentAsPercentageOfTheUnsoldShare(),
    };
}
