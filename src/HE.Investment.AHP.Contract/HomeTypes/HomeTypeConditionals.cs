using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes;

public record HomeTypeConditionals(
    YesNoType LocalCommissioningBodiesConsulted,
    YesNoType ShortStayAccommodation,
    RevenueFundingType RevenueFundingType,
    BuildingType BuildingType);
