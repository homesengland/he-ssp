using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Enums;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Common.Extensions;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveOlderPeopleHousingTypeCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveOlderPeopleHousingTypeCommand>
{
    public SaveOlderPeopleHousingTypeCommandHandler(IHomeTypeRepository homeTypeRepository, ILogger<SaveOlderPeopleHousingTypeCommandHandler> logger)
        : base(homeTypeRepository, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.OlderPeople };

    protected override IEnumerable<Action<SaveOlderPeopleHousingTypeCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveOlderPeopleHousingTypeCommand request, IHomeTypeEntity homeType) =>
            homeType.OlderPeopleHomeTypeDetails.ChangeHousingType(request.HousingType.MapTo<OlderPeopleHousingType>()),
    };
}
