using System.ComponentModel;

namespace HE.Investments.AHP.Allocation.Contract.Claims.Enum;

public enum MilestoneType
{
    Undefined,

    [Description("Acquisition")]
    Acquisition,

    [Description("Start on site")]
    StartOnSite,

    [Description("Practical completion")]
    Completion,
}
