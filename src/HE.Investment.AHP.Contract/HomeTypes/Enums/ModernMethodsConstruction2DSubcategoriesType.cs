using System.ComponentModel;

namespace HE.Investment.AHP.Contract.HomeTypes.Enums;

public enum ModernMethodsConstruction2DSubcategoriesType
{
    Undefined = 0,

    [Description("Basic framing only including walls, floors, stairs and roof")]
    BasicFramingOnly,

    [Description("Enhanced consolidation, for example insulation or internal linings ")]
    EnhancedConsolidation,

    [Description("Further enhanced consolidation, including: insulation, linings, external, cladding, roofing, doors, windows")]
    FurtherEnhancedConsolidation,
}
