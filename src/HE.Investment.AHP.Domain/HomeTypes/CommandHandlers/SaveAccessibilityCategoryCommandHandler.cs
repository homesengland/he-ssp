using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveAccessibilityCategoryCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveAccessibilityCategoryCommand>
{
    public SaveAccessibilityCategoryCommandHandler(IHomeTypeRepository homeTypeRepository, ILogger<SaveAccessibilityCategoryCommandHandler> logger)
        : base(homeTypeRepository, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.HomeInformation };

    protected override IEnumerable<Action<SaveAccessibilityCategoryCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveAccessibilityCategoryCommand request, IHomeTypeEntity homeType) => homeType.HomeInformation.ChangeAccessibilityCategory(request.AccessibilityCategory),
    };
}
