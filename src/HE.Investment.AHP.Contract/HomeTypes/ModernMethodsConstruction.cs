using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Contract.Site;

namespace HE.Investment.AHP.Contract.HomeTypes;

public record ModernMethodsConstruction(
        string ApplicationName,
        string HomeTypeName,
        SiteUsingModernMethodsOfConstruction SiteUsingModernMethodsOfConstruction,
        YesNoType ModernMethodsConstructionApplied,
        IList<ModernMethodsConstructionCategoriesType> ModernMethodsConstructionCategories,
        IList<ModernMethodsConstruction2DSubcategoriesType> ModernMethodsConstruction2DSubcategories,
        IList<ModernMethodsConstruction3DSubcategoriesType> ModernMethodsConstruction3DSubcategories)
    : HomeTypeSegmentBase(ApplicationName, HomeTypeName);
