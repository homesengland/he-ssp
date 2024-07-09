using System.ComponentModel;

namespace HE.Investments.AHP.Allocation.Contract.Claims.Enum;

public enum MilestoneStatus
{
    Undefined,
    [Description("Due soon")]
    DueSoon,
    Due,
    Overdue,
    Submitted,
    [Description("Under review")]
    UnderReview,
    Approved,
    Rejected,
    Reclaimed,
}
