using System.ComponentModel;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Enums;

public enum MilestoneStatus
{
    Undefined,
    ClaimDoesNotExist,
    Draft,
    Submitted,
    [Description("Under review")]
    UnderReview,
    Approved,
    Rejected,
    Paid,
}
