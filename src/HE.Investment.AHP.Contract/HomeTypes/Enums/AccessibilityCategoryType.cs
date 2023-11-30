using System.ComponentModel;

namespace HE.Investment.AHP.Contract.HomeTypes.Enums;

public enum AccessibilityCategoryType
{
    Undefined = 0,

    [Description("M4(1) Category 1: Visitable dwellings")]
    VisitableDwellings,

    [Description("M4(2) Category 2: Accessible and adaptable dwellings")]
    AccessibleAndAdaptableDwellings,

    [Description("M4(3) Category 3: Wheelchair user dwellings")]
    WheelchairUserDwellings,
}
