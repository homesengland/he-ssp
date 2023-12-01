using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveMoveOnAccommodationCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveMoveOnAccommodationCommand>
{
    public SaveMoveOnAccommodationCommandHandler(IHomeTypeRepository homeTypeRepository, ILogger<SaveMoveOnAccommodationCommandHandler> logger)
        : base(homeTypeRepository, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.HomeInformation };

    protected override IEnumerable<Action<SaveMoveOnAccommodationCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveMoveOnAccommodationCommand request, IHomeTypeEntity entity) =>
            entity.HomeInformation.ChangeIntendedAsMoveOnAccommodation(request.IntendedAsMoveOnAccommodation),
    };
}
