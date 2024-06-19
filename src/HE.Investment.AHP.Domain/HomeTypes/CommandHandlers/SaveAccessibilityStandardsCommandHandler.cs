using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveAccessibilityStandardsCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveAccessibilityStandardsCommand>
{
    public SaveAccessibilityStandardsCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IConsortiumUserContext accountUserContext,
        ILogger<SaveAccessibilityStandardsCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => [HomeTypeSegmentType.HomeInformation];

    protected override IEnumerable<Action<SaveAccessibilityStandardsCommand, IHomeTypeEntity>> SaveActions =>
    [
        (SaveAccessibilityStandardsCommand request, IHomeTypeEntity homeType) => homeType.HomeInformation.ChangeAccessibilityStandards(request.AccessibilityStandards),
    ];
}
