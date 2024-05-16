using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveAccessibilityCategoryCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveAccessibilityCategoryCommand>
{
    public SaveAccessibilityCategoryCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IAccountUserContext accountUserContext,
        ILogger<SaveAccessibilityCategoryCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => [HomeTypeSegmentType.HomeInformation];

    protected override IEnumerable<Action<SaveAccessibilityCategoryCommand, IHomeTypeEntity>> SaveActions =>
    [
        (SaveAccessibilityCategoryCommand request, IHomeTypeEntity homeType) => homeType.HomeInformation.ChangeAccessibilityCategory(request.AccessibilityCategory),
    ];
}
