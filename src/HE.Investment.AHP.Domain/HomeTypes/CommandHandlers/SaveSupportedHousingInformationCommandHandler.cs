using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveSupportedHousingInformationCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveSupportedHousingInformationCommand>
{
    public SaveSupportedHousingInformationCommandHandler(IHomeTypeRepository homeTypeRepository, ILogger<SaveSupportedHousingInformationCommandHandler> logger)
        : base(homeTypeRepository, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.SupportedHousingInformation };

    protected override IEnumerable<Action<SaveSupportedHousingInformationCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (SaveSupportedHousingInformationCommand request, IHomeTypeEntity homeType) => homeType.SupportedHousingInformation.ChangeLocalCommissioningBodiesConsulted(request.LocalCommissioningBodiesConsulted),
        (request, homeType) => homeType.SupportedHousingInformation.ChangeShortStayAccommodation(request.ShortStayAccommodation),
        (request, homeType) => homeType.SupportedHousingInformation.ChangeRevenueFundingType(request.RevenueFundingType),
    };
}
