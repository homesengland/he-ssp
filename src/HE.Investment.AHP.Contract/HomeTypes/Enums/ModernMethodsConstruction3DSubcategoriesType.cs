using System.ComponentModel;

namespace HE.Investment.AHP.Contract.HomeTypes.Enums;

public enum ModernMethodsConstruction3DSubcategoriesType
{
    Undefined = 0,

    [Description("Structural chassis only, without internal fittings")]
    StructuralChassisOnly,

    [Description("Structural chassis and internally fitted out")]
    StructuralChassisAndInternallyFittedOut,

    [Description("Structural chassis, internally fitted out and external cladding or roofing completed")]
    StructuralChassisInternallyFittedOutAndExternalCladdingOrRoofingCompleted,

    [Description("Structural chassis, internally fitted out and ‘podded room assembled’ - for example, bathrooms and kitchens")]
    StructuralChassisInternallyFittedOutAndPoddedRoomAssembled,
}
