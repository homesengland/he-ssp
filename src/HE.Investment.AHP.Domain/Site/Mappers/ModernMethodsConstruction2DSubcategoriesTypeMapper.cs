using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.Site.Mappers;

public class ModernMethodsConstruction2DSubcategoriesTypeMapper : EnumMapper<ModernMethodsConstruction2DSubcategoriesType>
{
    protected override IDictionary<ModernMethodsConstruction2DSubcategoriesType, int?> Mapping =>
        new Dictionary<ModernMethodsConstruction2DSubcategoriesType, int?>
        {
            { ModernMethodsConstruction2DSubcategoriesType.BasicFramingOnly, (int)invln_MMCcategory2subcategories._2abasicframingonlyinclwallsfloorsstairsroof },
            { ModernMethodsConstruction2DSubcategoriesType.EnhancedConsolidation, (int)invln_MMCcategory2subcategories._2benhancedconsolidationinsulationinternalliningsetc },
            { ModernMethodsConstruction2DSubcategoriesType.FurtherEnhancedConsolidation, (int)invln_MMCcategory2subcategories._2cfurtherenhancedconsolidationinsulationliningsexternalcladdingroofingdoorswindows },
        };
}
