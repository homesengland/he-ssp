using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveRevenueFundingCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveRevenueFundingCommand>
{
    public SaveRevenueFundingCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IAccountUserContext accountUserContext,
        ILogger<SaveRevenueFundingCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => [HomeTypeSegmentType.SupportedHousingInformation];

    protected override IEnumerable<Action<SaveRevenueFundingCommand, IHomeTypeEntity>> SaveActions =>
    [
        (SaveRevenueFundingCommand request, IHomeTypeEntity homeType) => homeType.SupportedHousingInformation.ChangeSources(request.Sources),
    ];
}
