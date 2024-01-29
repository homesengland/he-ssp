using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;

public class OutlinePlanningApprovalGrantedPlanningDetails : PlanningDetails, IQuestion
{
    public OutlinePlanningApprovalGrantedPlanningDetails(
        ReferenceNumber? referenceNumber = null,
        RequiredFurtherSteps? requiredFurtherSteps = null,
        ExpectedPlanningApprovalDate? expectedPlanningApprovalDate = null,
        OutlinePlanningApprovalDate? outlinePlanningApprovalDate = null,
        bool? isGrantFundingForAllHomes = null)
        : base(
            referenceNumber,
            requiredFurtherSteps: requiredFurtherSteps,
            expectedPlanningApprovalDate: expectedPlanningApprovalDate,
            outlinePlanningApprovalDate: outlinePlanningApprovalDate,
            isGrantFundingForAllHomes: isGrantFundingForAllHomes)
    {
    }

    public override SitePlanningStatus? PlanningStatus => SitePlanningStatus.OutlinePlanningApprovalGranted;

    public override bool IsAnswered()
    {
        return ReferenceNumber.IsProvided() &&
               RequiredFurtherSteps.IsProvided() &&
               ExpectedPlanningApprovalDate.IsProvided() &&
               OutlinePlanningApprovalDate.IsProvided() &&
               IsGrantFundingForAllHomes.IsProvided();
    }
}
