using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveExemptFromTheRightToSharedOwnershipCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveExemptFromTheRightToSharedOwnershipCommand>
{
    public SaveExemptFromTheRightToSharedOwnershipCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IConsortiumUserContext accountUserContext,
        ILogger<SaveExemptFromTheRightToSharedOwnershipCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => [HomeTypeSegmentType.TenureDetails];

    protected override IEnumerable<Action<SaveExemptFromTheRightToSharedOwnershipCommand, IHomeTypeEntity>> SaveActions =>
    [
        (SaveExemptFromTheRightToSharedOwnershipCommand request, IHomeTypeEntity entity) =>
            entity.TenureDetails.ChangeExemptFromTheRightToSharedOwnership(request.ExemptFromTheRightToSharedOwnership),
    ];
}
