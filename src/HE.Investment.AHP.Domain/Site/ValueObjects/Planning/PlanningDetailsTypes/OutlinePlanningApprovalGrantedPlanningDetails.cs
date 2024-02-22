using HE.Investment.AHP.Contract.Site;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;

public class OutlinePlanningApprovalGrantedPlanningDetails : PlanningDetails
{
    public OutlinePlanningApprovalGrantedPlanningDetails(
        ReferenceNumber? referenceNumber = null,
        RequiredFurtherSteps? requiredFurtherSteps = null,
        ExpectedPlanningApprovalDate? expectedPlanningApprovalDate = null,
        OutlinePlanningApprovalDate? outlinePlanningApprovalDate = null,
        bool? isGrantFundingForAllHomesCoveredByApplication = null)
        : base(
            referenceNumber,
            requiredFurtherSteps: requiredFurtherSteps,
            expectedPlanningApprovalDate: expectedPlanningApprovalDate,
            outlinePlanningApprovalDate: outlinePlanningApprovalDate,
            isGrantFundingForAllHomesCoveredByApplication: isGrantFundingForAllHomesCoveredByApplication)
    {
    }

    public override SitePlanningStatus? PlanningStatus => SitePlanningStatus.OutlinePlanningApprovalGranted;

    protected override IReadOnlyCollection<string> ActiveFields => new[]
    {
        nameof(ReferenceNumber),
        nameof(RequiredFurtherSteps),
        nameof(ExpectedPlanningApprovalDate),
        nameof(OutlinePlanningApprovalDate),
        nameof(IsGrantFundingForAllHomesCoveredByApplication),
    };
}
