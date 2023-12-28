using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveExemptFromTheRightToSharedOwnershipCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveExemptFromTheRightToSharedOwnershipCommand>
{
    public SaveExemptFromTheRightToSharedOwnershipCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IAccountUserContext accountUserContext,
        ILogger<SaveExemptFromTheRightToSharedOwnershipCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.TenureDetails };

    protected override IEnumerable<Action<SaveExemptFromTheRightToSharedOwnershipCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveExemptFromTheRightToSharedOwnershipCommand request, IHomeTypeEntity entity) =>
            entity.TenureDetails.ChangeExemptFromTheRightToSharedOwnership(request.ExemptFromTheRightToSharedOwnership),
    };
}
