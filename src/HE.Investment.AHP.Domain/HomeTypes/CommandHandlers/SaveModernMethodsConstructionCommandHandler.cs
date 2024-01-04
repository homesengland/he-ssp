using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveModernMethodsConstructionCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveModernMethodsConstructionCommand>
{
    public SaveModernMethodsConstructionCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IAccountUserContext accountUserContext,
        ILogger<SaveModernMethodsConstructionCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.ModernMethodsConstruction };

    protected override IEnumerable<Action<SaveModernMethodsConstructionCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveModernMethodsConstructionCommand request, IHomeTypeEntity entity) =>
            entity.ModernMethodsConstruction.ChangeModernMethodsConstructionApplied(request.ModernMethodsConstructionApplied),
    };
}
