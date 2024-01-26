using System.ComponentModel;

namespace HE.Investment.AHP.Contract.Site;

public enum SitePlanningStatus
{
    Undefined,
    [Description("Detailed planning approval granted")]
    DetailedPlanningApprovalGranted,
    [Description("Detailed planning approval granted with some further steps required before start on site can occur")]
    DetailedPlanningApprovalGrantedWithFurtherSteps,
    [Description("Detailed planning application submitted")]
    DetailedPlanningApplicationSubmitted,
    [Description("Outline planning approval granted")]
    OutlinePlanningApprovalGranted,
    [Description("Outline planning application submitted")]
    OutlinePlanningApplicationSubmitted,
    [Description("Planning discussions underway with the planning office")]
    PlanningDiscussionsUnderwayWithThePlanningOffice,
    [Description("No progress on planning application")]
    NoProgressOnPlanningApplication,
    [Description("No planning required")]
    NoPlanningRequired,
}
