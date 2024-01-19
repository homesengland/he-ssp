using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveSupportedHousingInformationCommand(
        AhpApplicationId ApplicationId,
        HomeTypeId HomeTypeId,
        YesNoType LocalCommissioningBodiesConsulted,
        YesNoType ShortStayAccommodation,
        RevenueFundingType RevenueFundingType)
    : ISaveHomeTypeSegmentCommand;
