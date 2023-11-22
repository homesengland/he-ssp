using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveSupportedHousingInformationCommand(
        string ApplicationId,
        string HomeTypeId,
        YesNoType LocalCommissioningBodiesConsulted,
        YesNoType ShortStayAccommodation,
        RevenueFundingType RevenueFundingType)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
