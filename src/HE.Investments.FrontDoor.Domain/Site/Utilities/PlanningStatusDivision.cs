using HE.Investments.Common.Contract.Enum;

namespace HE.Investments.FrontDoor.Domain.Site.Utilities;

public static class PlanningStatusDivision
{
    public static bool IsStatusAllowedForLoanApplication(SitePlanningStatus status)
    {
        return status is SitePlanningStatus.DetailedPlanningApprovalGranted
               or SitePlanningStatus.DetailedPlanningApprovalGrantedWithFurtherSteps
               or SitePlanningStatus.DetailedPlanningApplicationSubmitted
               or SitePlanningStatus.OutlinePlanningApprovalGranted
               or SitePlanningStatus.OutlinePlanningApplicationSubmitted
               or SitePlanningStatus.PlanningDiscussionsUnderwayWithThePlanningOffice;
    }
}
