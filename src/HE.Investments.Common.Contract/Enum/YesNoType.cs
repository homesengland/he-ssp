using System.ComponentModel;

namespace HE.Investments.Common.Contract.Enum;

public enum YesNoType
{
    Undefined = 0,

    [Description("Yes")]
    Yes,

    [Description("No")]
    No,
}
