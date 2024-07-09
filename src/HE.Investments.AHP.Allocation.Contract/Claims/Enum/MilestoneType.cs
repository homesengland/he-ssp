using System.ComponentModel;

namespace HE.Investments.AHP.Allocation.Contract.Claims.Enum;

public enum MilestoneType
{
    Undefined,

    [Description("Acquisition milestone")]
    Acquisition,

    [Description("Start on site milestone")]
    StartOnSite,

    [Description("Practical completion milestone")]
    Completion,
}
