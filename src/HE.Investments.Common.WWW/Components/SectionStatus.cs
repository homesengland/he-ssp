using System.ComponentModel;

namespace HE.Investments.Common.WWW.Components;

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
    [Description("Cannot Start Yet")]
    CannotStartYet,
}
