using System.ComponentModel;

namespace HE.Investment.AHP.Contract.Common.Enums;

public enum IsSectionCompleted
{
    Undefied = 0,

    [Description("Yes, I've completed this section")]
    Yes,

    [Description("No, I'll come back later")]
    No,
}
