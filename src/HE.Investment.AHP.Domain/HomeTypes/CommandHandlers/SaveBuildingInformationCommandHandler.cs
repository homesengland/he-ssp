using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveBuildingInformationCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveBuildingInformationCommand>
{
    public SaveBuildingInformationCommandHandler(IHomeTypeRepository homeTypeRepository, ILogger<SaveBuildingInformationCommandHandler> logger)
        : base(homeTypeRepository, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.HomeInformation };

    protected override IEnumerable<Action<SaveBuildingInformationCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveBuildingInformationCommand request, IHomeTypeEntity entity) =>
            entity.HomeInformation.ChangeBuildingType(request.BuildingType),
    };
}
