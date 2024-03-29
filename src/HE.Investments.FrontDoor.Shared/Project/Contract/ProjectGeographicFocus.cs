using System.ComponentModel;

namespace HE.Investments.FrontDoor.Shared.Project.Contract;

public enum ProjectGeographicFocus
{
    Undefined = 0,
    National,
    Regional,
    [Description("Specific local authority")]
    SpecificLocalAuthority,
    [Description("I do not know")]
    Unknown,
}
