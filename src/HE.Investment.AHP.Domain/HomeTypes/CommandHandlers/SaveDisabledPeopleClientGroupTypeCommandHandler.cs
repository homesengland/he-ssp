using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveDisabledPeopleClientGroupTypeCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveDisabledPeopleClientGroupTypeCommand>
{
    public SaveDisabledPeopleClientGroupTypeCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IAccountUserContext accountUserContext,
        ILogger<SaveDisabledPeopleClientGroupTypeCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.DisabledAndVulnerablePeople };

    protected override IEnumerable<Action<SaveDisabledPeopleClientGroupTypeCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveDisabledPeopleClientGroupTypeCommand request, IHomeTypeEntity homeType) =>
            homeType.DisabledPeopleHomeTypeDetails.ChangeClientGroupType(request.ClientGroupType),
    };
}
