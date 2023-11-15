using System.ComponentModel;

namespace HE.Investment.AHP.Contract.Application;

public enum SectionStatus
{
    Undefined = 0,
    [Description("Not Started")]
    NotStarted,
    [Description("In Progress")]
    InProgress,
    Completed,
}
