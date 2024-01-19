using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveMoveOnAccommodationCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveMoveOnAccommodationCommand>
{
    public SaveMoveOnAccommodationCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IAccountUserContext accountUserContext,
        ILogger<SaveMoveOnAccommodationCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.HomeInformation };

    protected override IEnumerable<Action<SaveMoveOnAccommodationCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveMoveOnAccommodationCommand request, IHomeTypeEntity entity) =>
            entity.HomeInformation.ChangeIntendedAsMoveOnAccommodation(request.IntendedAsMoveOnAccommodation),
    };
}
