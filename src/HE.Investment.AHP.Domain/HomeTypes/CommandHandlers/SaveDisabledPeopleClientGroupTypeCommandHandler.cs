using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Enums;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Common.Extensions;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveDisabledPeopleClientGroupTypeCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveDisabledPeopleClientGroupTypeCommand>
{
    public SaveDisabledPeopleClientGroupTypeCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        ILogger<SaveDisabledPeopleClientGroupTypeCommandHandler> logger)
        : base(homeTypeRepository, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.DisabledAndVulnerablePeople };

    protected override IEnumerable<Action<SaveDisabledPeopleClientGroupTypeCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveDisabledPeopleClientGroupTypeCommand request, IHomeTypeEntity homeType) =>
            homeType.DisabledPeopleHomeTypeDetails.ChangeClientGroupType(request.ClientGroupType.MapTo<DisabledPeopleClientGroupType>()),
    };
}
