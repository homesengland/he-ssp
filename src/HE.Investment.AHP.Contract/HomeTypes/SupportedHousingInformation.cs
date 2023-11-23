using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes;

public record SupportedHousingInformation(
    string? HomeTypeName,
    YesNoType LocalCommissioningBodiesConsulted,
    YesNoType ShortStayAccommodation,
    RevenueFundingType RevenueFundingType,
    IList<RevenueFundingSourceType> RevenueFundingSources);
