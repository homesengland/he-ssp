using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveMoveOnArrangementsCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveMoveOnArrangementsCommand>
{
    public SaveMoveOnArrangementsCommandHandler(IHomeTypeRepository homeTypeRepository, ILogger<SaveMoveOnArrangementsCommandHandler> logger)
        : base(homeTypeRepository, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.SupportedHousingInformation };

    protected override IEnumerable<Action<SaveMoveOnArrangementsCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveMoveOnArrangementsCommand request, IHomeTypeEntity homeType) =>
        {
            var moveOnArrangements = string.IsNullOrEmpty(request.MoveOnArrangements) ? null : new MoreInformation(request.MoveOnArrangements);
            homeType.SupportedHousingInformation.ChangeMoveOnArrangements(moveOnArrangements);
        },
    };
}
