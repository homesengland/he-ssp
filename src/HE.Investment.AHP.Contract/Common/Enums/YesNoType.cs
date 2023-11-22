using System.ComponentModel;

namespace HE.Investment.AHP.Contract.Common.Enums;

public enum YesNoType
{
    Undefined = 0,

    [Description("Yes")]
    Yes,

    [Description("No")]
    No,
}
