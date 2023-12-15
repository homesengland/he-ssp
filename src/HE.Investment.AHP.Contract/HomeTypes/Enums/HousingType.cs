using System.ComponentModel;

namespace HE.Investment.AHP.Contract.HomeTypes.Enums;

public enum HousingType
{
    Undefined = 0,

    [Description("General")]
    General,

    [Description("Homes for older people")]
    HomesForOlderPeople,

    [Description("Homes for disabled and vulnerable people")]
    HomesForDisabledAndVulnerablePeople,
}
