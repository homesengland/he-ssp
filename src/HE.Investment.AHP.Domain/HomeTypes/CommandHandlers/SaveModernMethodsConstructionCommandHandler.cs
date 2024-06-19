using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveModernMethodsConstructionCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveModernMethodsConstructionCommand>
{
    public SaveModernMethodsConstructionCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IConsortiumUserContext accountUserContext,
        ILogger<SaveModernMethodsConstructionCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => [HomeTypeSegmentType.ModernMethodsConstruction];

    protected override IEnumerable<Action<SaveModernMethodsConstructionCommand, IHomeTypeEntity>> SaveActions =>
    [
        (SaveModernMethodsConstructionCommand request, IHomeTypeEntity entity) =>
            entity.ModernMethodsConstruction.ChangeModernMethodsConstructionApplied(request.ModernMethodsConstructionApplied),
    ];
}
