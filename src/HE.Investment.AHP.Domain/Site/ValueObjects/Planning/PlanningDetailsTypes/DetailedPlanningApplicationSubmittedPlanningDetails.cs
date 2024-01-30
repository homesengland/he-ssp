using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;

public class DetailedPlanningApplicationSubmittedPlanningDetails : PlanningDetails, IQuestion
{
    public DetailedPlanningApplicationSubmittedPlanningDetails(
        ReferenceNumber? referenceNumber = null,
        RequiredFurtherSteps? requiredFurtherSteps = null,
        ApplicationForDetailedPlanningSubmittedDate? applicationForDetailedPlanningSubmittedDate = null,
        ExpectedPlanningApprovalDate? expectedPlanningApprovalDate = null,
        bool? isGrantFundingForAllHomes = null)
        : base(
            referenceNumber,
            requiredFurtherSteps: requiredFurtherSteps,
            applicationForDetailedPlanningSubmittedDate: applicationForDetailedPlanningSubmittedDate,
            expectedPlanningApprovalDate: expectedPlanningApprovalDate,
            isGrantFundingForAllHomes: isGrantFundingForAllHomes)
    {
    }

    public override SitePlanningStatus? PlanningStatus => SitePlanningStatus.DetailedPlanningApplicationSubmitted;

    public override bool IsAnswered()
    {
        return ReferenceNumber.IsProvided() &&
               RequiredFurtherSteps.IsProvided() &&
               ApplicationForDetailedPlanningSubmittedDate.IsProvided() &&
               ExpectedPlanningApprovalDate.IsProvided() &&
               IsGrantFundingForAllHomes.IsProvided();
    }
}
