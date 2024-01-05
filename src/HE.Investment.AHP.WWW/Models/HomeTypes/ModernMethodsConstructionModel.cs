using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class ModernMethodsConstructionModel : HomeTypeBasicModel
{
    public ModernMethodsConstructionModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public ModernMethodsConstructionModel()
        : this(string.Empty, string.Empty)
    {
    }

    public YesNoType ModernMethodsConstructionApplied { get; set; }

    public IList<ModernMethodsConstructionCategoriesType>? ModernMethodsConstructionCategories { get; set; }

    public IList<ModernMethodsConstruction2DSubcategoriesType>? ModernMethodsConstruction2DSubcategories { get; set; }

    public IList<ModernMethodsConstruction3DSubcategoriesType>? ModernMethodsConstruction3DSubcategories { get; set; }
}
