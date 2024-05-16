using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveSupportedHousingInformationCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveSupportedHousingInformationCommand>
{
    public SaveSupportedHousingInformationCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IAccountUserContext accountUserContext,
        ILogger<SaveSupportedHousingInformationCommandHandler> logger)
        : base(homeTypeRepository, accountUserContext, logger)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => [HomeTypeSegmentType.SupportedHousingInformation];

    protected override IEnumerable<Action<SaveSupportedHousingInformationCommand, IHomeTypeEntity>> SaveActions =>
    [
        (SaveSupportedHousingInformationCommand request, IHomeTypeEntity homeType) => homeType.SupportedHousingInformation.ChangeLocalCommissioningBodiesConsulted(request.LocalCommissioningBodiesConsulted),
        (request, homeType) => homeType.SupportedHousingInformation.ChangeShortStayAccommodation(request.ShortStayAccommodation),
        (request, homeType) => homeType.SupportedHousingInformation.ChangeRevenueFundingType(request.RevenueFundingType),
    ];
}
