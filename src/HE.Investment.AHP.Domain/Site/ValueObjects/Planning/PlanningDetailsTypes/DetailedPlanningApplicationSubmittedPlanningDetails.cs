using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;

public class DetailedPlanningApplicationSubmittedPlanningDetails : PlanningDetails
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
            isGrantFundingForAllHomesCoveredByApplication: isGrantFundingForAllHomes)
    {
    }

    public override SitePlanningStatus? PlanningStatus => SitePlanningStatus.DetailedPlanningApplicationSubmitted;

    protected override IReadOnlyCollection<string> ActiveFields => new[]
    {
        nameof(ReferenceNumber),
        nameof(RequiredFurtherSteps),
        nameof(ApplicationForDetailedPlanningSubmittedDate),
        nameof(ExpectedPlanningApprovalDate),
        nameof(IsGrantFundingForAllHomesCoveredByApplication),
    };
}
