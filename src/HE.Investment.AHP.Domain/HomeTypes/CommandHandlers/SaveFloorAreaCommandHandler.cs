using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveFloorAreaCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveFloorAreaCommand>
{
    public SaveFloorAreaCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IConsortiumUserContext accountUserContext,
        ILogger<SaveFloorAreaCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => [HomeTypeSegmentType.HomeInformation];

    protected override IEnumerable<Action<SaveFloorAreaCommand, IHomeTypeEntity>> SaveActions =>
    [
        (SaveFloorAreaCommand request, IHomeTypeEntity entity) =>
            entity.HomeInformation.ChangeInternalFloorArea(request.FloorArea),
        (request, entity) =>
            entity.HomeInformation.ChangeMeetNationallyDescribedSpaceStandards(request.MeetNationallyDescribedSpaceStandards),
    ];
}
