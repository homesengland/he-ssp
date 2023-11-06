using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveHomeInformationCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveHomeInformationCommand>
{
    public SaveHomeInformationCommandHandler(IHomeTypeRepository homeTypeRepository, ILogger<SaveHomeInformationCommandHandler> logger)
        : base(homeTypeRepository, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.HomeInformation };

    protected override IEnumerable<Action<SaveHomeInformationCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveHomeInformationCommand request, IHomeTypeEntity homeType) => homeType.HomeInformation.ChangeNumberOfHomes(request.NumberOfHomes),
        (request, homeType) => homeType.HomeInformation.ChangeNumberOfBedrooms(request.NumberOfBedrooms),
        (request, homeType) => homeType.HomeInformation.ChangeMaximumOccupancy(request.MaximumOccupancy),
        (request, homeType) => homeType.HomeInformation.ChangeNumberOfStoreys(request.NumberOfStoreys),
    };
}
