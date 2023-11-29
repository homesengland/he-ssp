using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveCustomBuildPropertyCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveCustomBuildPropertyCommand>
{
    public SaveCustomBuildPropertyCommandHandler(IHomeTypeRepository homeTypeRepository, ILogger<SaveCustomBuildPropertyCommandHandler> logger)
        : base(homeTypeRepository, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.HomeInformation };

    protected override IEnumerable<Action<SaveCustomBuildPropertyCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveCustomBuildPropertyCommand request, IHomeTypeEntity homeType) => homeType.HomeInformation.ChangeCustomBuild(request.CustomBuild),
    };
}
