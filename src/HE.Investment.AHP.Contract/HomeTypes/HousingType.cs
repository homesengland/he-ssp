using System.ComponentModel;

namespace HE.Investment.AHP.Contract.HomeTypes;

public enum HousingType
{
    Undefined = 0,

    [Description("General")]
    General,

    [Description("Housing for older people")]
    HomesForOlderPeople,

    [Description("Housing for disabled and vulnerable people")]
    HomesForDisabledAndVulnerablePeople,
}
