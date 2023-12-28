using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveAccessibilityStandardsCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveAccessibilityStandardsCommand>
{
    public SaveAccessibilityStandardsCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IAccountUserContext accountUserContext,
        ILogger<SaveAccessibilityStandardsCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.HomeInformation };

    protected override IEnumerable<Action<SaveAccessibilityStandardsCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveAccessibilityStandardsCommand request, IHomeTypeEntity homeType) => homeType.HomeInformation.ChangeAccessibilityStandards(request.AccessibilityStandards),
    };
}
