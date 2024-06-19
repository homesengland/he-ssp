using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveBuildingInformationCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveBuildingInformationCommand>
{
    public SaveBuildingInformationCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IConsortiumUserContext accountUserContext,
        ILogger<SaveBuildingInformationCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => [HomeTypeSegmentType.HomeInformation];

    protected override IEnumerable<Action<SaveBuildingInformationCommand, IHomeTypeEntity>> SaveActions =>
    [
        (SaveBuildingInformationCommand request, IHomeTypeEntity entity) =>
            entity.HomeInformation.ChangeBuildingType(request.BuildingType),
    ];
}
