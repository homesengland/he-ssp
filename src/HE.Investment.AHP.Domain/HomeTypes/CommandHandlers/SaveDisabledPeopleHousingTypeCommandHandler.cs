using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveDisabledPeopleHousingTypeCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveDisabledPeopleHousingTypeCommand>
{
    public SaveDisabledPeopleHousingTypeCommandHandler(IHomeTypeRepository homeTypeRepository, ILogger<SaveDisabledPeopleHousingTypeCommandHandler> logger)
        : base(homeTypeRepository, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.DisabledAndVulnerablePeople };

    protected override IEnumerable<Action<SaveDisabledPeopleHousingTypeCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveDisabledPeopleHousingTypeCommand request, IHomeTypeEntity homeType) =>
            homeType.DisabledPeopleHomeTypeDetails.ChangeHousingType(request.HousingType),
    };
}
