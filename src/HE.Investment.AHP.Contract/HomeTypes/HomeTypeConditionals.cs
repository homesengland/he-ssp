using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.Contract.HomeTypes;

public record HomeTypeConditionals(
    YesNoType ShortStayAccommodation,
    RevenueFundingType RevenueFundingType,
    BuildingType BuildingType,
    YesNoType AccessibleStandards,
    YesNoType MeetNationallyDescribedSpaceStandards,
    YesNoType ExemptFromTheRightToSharedOwnership,
    bool IsProspectiveRentIneligible,
    SiteUsingModernMethodsOfConstruction SiteUsingModernMethodsOfConstruction,
    YesNoType ModernMethodsConstructionApplied,
    IList<ModernMethodsConstructionCategoriesType> ModernMethodsConstructionCategories);
