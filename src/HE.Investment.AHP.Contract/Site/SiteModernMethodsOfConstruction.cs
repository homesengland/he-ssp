using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Contract.Site;

public record SiteModernMethodsOfConstruction(
    SiteUsingModernMethodsOfConstruction? UsingModernMethodsOfConstruction,
    IList<ModernMethodsConstructionCategoriesType>? ModernMethodsConstructionCategories,
    IList<ModernMethodsConstruction2DSubcategoriesType>? ModernMethodsConstruction2DSubcategories,
    IList<ModernMethodsConstruction3DSubcategoriesType>? ModernMethodsConstruction3DSubcategories,
    string? FutureAdoptionPlans,
    string? FutureAdoptionExpectedImpact,
    string? InformationBarriers,
    string? InformationImpact);
