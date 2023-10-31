using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveDisablePeopleHousingTypeCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveDisablePeopleHousingTypeCommand>
{
    public SaveDisablePeopleHousingTypeCommandHandler(IHomeTypeRepository homeTypeRepository, ILogger<SaveDisablePeopleHousingTypeCommandHandler> logger)
        : base(homeTypeRepository, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.DisabledAndVulnerablePeople };

    protected override IEnumerable<Action<SaveDisablePeopleHousingTypeCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveDisablePeopleHousingTypeCommand request, IHomeTypeEntity homeType) =>
            homeType.DisabledPeopleHomeTypeDetails.ChangeHousingType(request.HousingType),
    };
}
