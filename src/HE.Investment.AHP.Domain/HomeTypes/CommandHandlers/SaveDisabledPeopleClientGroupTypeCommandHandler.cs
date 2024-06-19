using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveDisabledPeopleClientGroupTypeCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveDisabledPeopleClientGroupTypeCommand>
{
    public SaveDisabledPeopleClientGroupTypeCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IConsortiumUserContext accountUserContext,
        ILogger<SaveDisabledPeopleClientGroupTypeCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => [HomeTypeSegmentType.DisabledAndVulnerablePeople];

    protected override IEnumerable<Action<SaveDisabledPeopleClientGroupTypeCommand, IHomeTypeEntity>> SaveActions =>
    [
        (SaveDisabledPeopleClientGroupTypeCommand request, IHomeTypeEntity homeType) =>
            homeType.DisabledPeopleHomeTypeDetails.ChangeClientGroupType(request.ClientGroupType),
    ];
}
