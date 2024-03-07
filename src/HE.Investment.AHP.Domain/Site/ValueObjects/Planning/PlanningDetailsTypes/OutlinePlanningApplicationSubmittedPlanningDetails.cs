using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;

public class OutlinePlanningApplicationSubmittedPlanningDetails : PlanningDetails
{
    public OutlinePlanningApplicationSubmittedPlanningDetails(
        ReferenceNumber? referenceNumber = null,
        RequiredFurtherSteps? requiredFurtherSteps = null,
        ExpectedPlanningApprovalDate? expectedPlanningApprovalDate = null,
        bool? isGrantFundingForAllHomes = null,
        PlanningSubmissionDate? planningSubmissionDate = null)
        : base(
            referenceNumber,
            requiredFurtherSteps: requiredFurtherSteps,
            expectedPlanningApprovalDate: expectedPlanningApprovalDate,
            isGrantFundingForAllHomesCoveredByApplication: isGrantFundingForAllHomes,
            planningSubmissionDate: planningSubmissionDate)
    {
    }

    public override SitePlanningStatus? PlanningStatus => SitePlanningStatus.OutlinePlanningApplicationSubmitted;

    protected override IReadOnlyCollection<string> ActiveFields => new[]
    {
        nameof(ReferenceNumber),
        nameof(RequiredFurtherSteps),
        nameof(ExpectedPlanningApprovalDate),
        nameof(IsGrantFundingForAllHomesCoveredByApplication),
        nameof(PlanningSubmissionDate),
    };
}
