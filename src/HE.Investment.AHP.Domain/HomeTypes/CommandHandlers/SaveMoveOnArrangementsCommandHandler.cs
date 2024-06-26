using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Consortium.Shared.UserContext;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveMoveOnArrangementsCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveMoveOnArrangementsCommand>
{
    public SaveMoveOnArrangementsCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IConsortiumUserContext accountUserContext,
        ILogger<SaveMoveOnArrangementsCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => [HomeTypeSegmentType.SupportedHousingInformation];

    protected override IEnumerable<Action<SaveMoveOnArrangementsCommand, IHomeTypeEntity>> SaveActions =>
    [
        (SaveMoveOnArrangementsCommand request, IHomeTypeEntity homeType) =>
        {
            var moveOnArrangements = request.MoveOnArrangements.IsNotProvided()
                ? null
                : new MoreInformation(request.MoveOnArrangements!, "move in arrangements");
            homeType.SupportedHousingInformation.ChangeMoveOnArrangements(moveOnArrangements);
        },
    ];
}
