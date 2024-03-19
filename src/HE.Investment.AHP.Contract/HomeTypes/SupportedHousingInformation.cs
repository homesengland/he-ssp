using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.Contract.HomeTypes;

public record SupportedHousingInformation(
        string ApplicationName,
        string HomeTypeName,
        YesNoType LocalCommissioningBodiesConsulted,
        YesNoType ShortStayAccommodation,
        RevenueFundingType RevenueFundingType,
        IList<RevenueFundingSourceType> RevenueFundingSources,
        string? MoveOnArrangements,
        string? TypologyLocationAndDesign,
        string? ExitPlan)
    : HomeTypeSegmentBase(ApplicationName, HomeTypeName);
