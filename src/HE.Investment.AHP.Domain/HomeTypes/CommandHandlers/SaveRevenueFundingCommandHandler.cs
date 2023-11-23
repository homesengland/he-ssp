using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveRevenueFundingCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveRevenueFundingCommand>
{
    public SaveRevenueFundingCommandHandler(IHomeTypeRepository homeTypeRepository, ILogger<SaveRevenueFundingCommandHandler> logger)
        : base(homeTypeRepository, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.SupportedHousingInformation };

    protected override IEnumerable<Action<SaveRevenueFundingCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveRevenueFundingCommand request, IHomeTypeEntity homeType) => homeType.SupportedHousingInformation.ChangeSources(request.Sources),
    };
}
