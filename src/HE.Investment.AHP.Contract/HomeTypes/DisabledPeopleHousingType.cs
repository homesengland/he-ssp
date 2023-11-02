using System.ComponentModel;

namespace HE.Investment.AHP.Contract.HomeTypes;

public enum DisabledPeopleHousingType
{
    Undefined = 0,

    [Description("Designated homes for disabled and vulnerable people with access to support")]
    DesignatedHomes,

    [Description("Designated supported homes for disabled and vulnerable people")]
    DesignatedSupportedHomes,

    [Description("Purpose-designed homes for disabled and vulnerable people with access to support")]
    PurposeDesignedHomes,

    [Description("Purpose-designed supported homes for disabled and vulnerable people")]
    PurposeDesignedSupportedHomes,
}
