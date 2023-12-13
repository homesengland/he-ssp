using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveFloorAreaCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveFloorAreaCommand>
{
    public SaveFloorAreaCommandHandler(IHomeTypeRepository homeTypeRepository, ILogger<SaveFloorAreaCommandHandler> logger)
        : base(homeTypeRepository, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.HomeInformation };

    protected override IEnumerable<Action<SaveFloorAreaCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveFloorAreaCommand request, IHomeTypeEntity entity) =>
            entity.HomeInformation.ChangeInternalFloorArea(request.FloorArea),
        (request, entity) =>
            entity.HomeInformation.ChangeMeetNationallyDescribedSpaceStandards(request.MeetNationallyDescribedSpaceStandards),
    };
}
