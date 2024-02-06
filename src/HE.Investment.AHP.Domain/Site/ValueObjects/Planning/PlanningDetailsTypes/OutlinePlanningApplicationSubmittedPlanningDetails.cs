using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;

public class OutlinePlanningApplicationSubmittedPlanningDetails : PlanningDetails, IQuestion
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

    public override bool IsAnswered()
    {
        return ReferenceNumber.IsProvided() &&
               RequiredFurtherSteps.IsProvided() &&
               ExpectedPlanningApprovalDate.IsProvided() &&
               IsGrantFundingForAllHomesCoveredByApplication.IsProvided() &&
               PlanningSubmissionDate.IsProvided();
    }
}
