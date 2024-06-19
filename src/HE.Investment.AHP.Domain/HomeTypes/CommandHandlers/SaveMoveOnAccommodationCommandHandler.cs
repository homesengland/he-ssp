using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveMoveOnAccommodationCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveMoveOnAccommodationCommand>
{
    public SaveMoveOnAccommodationCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IConsortiumUserContext accountUserContext,
        ILogger<SaveMoveOnAccommodationCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => [HomeTypeSegmentType.HomeInformation];

    protected override IEnumerable<Action<SaveMoveOnAccommodationCommand, IHomeTypeEntity>> SaveActions =>
    [
        (SaveMoveOnAccommodationCommand request, IHomeTypeEntity entity) =>
            entity.HomeInformation.ChangeIntendedAsMoveOnAccommodation(request.IntendedAsMoveOnAccommodation),
    ];
}
