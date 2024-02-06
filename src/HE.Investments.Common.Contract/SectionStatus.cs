using System.ComponentModel;

namespace HE.Investments.Common.Contract;

public enum SectionStatus
{
    Undefined,
    [Description("Not Started")]
    NotStarted,
    [Description("In Progress")]
    InProgress,
    Completed,
    Submitted,
    [Description("Not Submitted")]
    NotSubmitted,
    Withdrawn,
    [Description("On Hold")]
    OnHold,
}
