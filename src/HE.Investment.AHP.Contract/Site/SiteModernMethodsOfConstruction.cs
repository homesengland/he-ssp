using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Contract.Site;

public record SiteModernMethodsOfConstruction(
    SiteUsingModernMethodsOfConstruction? UsingModernMethodsOfConstruction = null,
    IList<ModernMethodsConstructionCategoriesType>? ModernMethodsConstructionCategories = null,
    IList<ModernMethodsConstruction2DSubcategoriesType>? ModernMethodsConstruction2DSubcategories = null,
    IList<ModernMethodsConstruction3DSubcategoriesType>? ModernMethodsConstruction3DSubcategories = null,
    string? FutureAdoptionPlans = null,
    string? FutureAdoptionExpectedImpact = null,
    string? InformationBarriers = null,
    string? InformationImpact = null);
