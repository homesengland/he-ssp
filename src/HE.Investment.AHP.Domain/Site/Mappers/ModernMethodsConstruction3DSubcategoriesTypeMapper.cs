using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.Site.Mappers;

public class ModernMethodsConstruction3DSubcategoriesTypeMapper : EnumMapper<ModernMethodsConstruction3DSubcategoriesType>
{
    protected override IDictionary<ModernMethodsConstruction3DSubcategoriesType, int?> Mapping =>
        new Dictionary<ModernMethodsConstruction3DSubcategoriesType, int?>
        {
            { ModernMethodsConstruction3DSubcategoriesType.StructuralChassisOnly, (int)invln_MMCcategory1subcategories._1astructuralchassisonlynotfittedout },
            { ModernMethodsConstruction3DSubcategoriesType.StructuralChassisAndInternallyFittedOut, (int)invln_MMCcategory1subcategories._1bstructuralchassisandinternalfitout },
            { ModernMethodsConstruction3DSubcategoriesType.StructuralChassisInternallyFittedOutAndExternalCladdingOrRoofingCompleted, (int)invln_MMCcategory1subcategories._1cstructuralchassisfitoutandexternalcladdingroofingcomplete },
            { ModernMethodsConstruction3DSubcategoriesType.StructuralChassisInternallyFittedOutAndPoddedRoomAssembled, (int)invln_MMCcategory1subcategories._1dstructuralchassisandinternalfitoutpoddedroomassemblesbathroomskitchensetc },
        };
}
