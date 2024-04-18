using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;

public class DetailedPlanningApprovalGrantedWithFurtherStepsPlanningDetails : PlanningDetails
{
    public DetailedPlanningApprovalGrantedWithFurtherStepsPlanningDetails(
        ReferenceNumber? referenceNumber = null,
        DetailedPlanningApprovalDate? detailedPlanningApprovalDate = null,
        RequiredFurtherSteps? requiredFurtherSteps = null,
        bool? isGrantFundingForAllHomes = null)
        : base(
            referenceNumber,
            detailedPlanningApprovalDate,
            requiredFurtherSteps: requiredFurtherSteps,
            isGrantFundingForAllHomesCoveredByApplication: isGrantFundingForAllHomes)
    {
    }

    public override SitePlanningStatus? PlanningStatus => SitePlanningStatus.DetailedPlanningApprovalGrantedWithFurtherSteps;

    protected override IReadOnlyCollection<string> ActiveFields => new[]
    {
        nameof(ReferenceNumber),
        nameof(DetailedPlanningApprovalDate),
        nameof(RequiredFurtherSteps),
        nameof(IsGrantFundingForAllHomesCoveredByApplication),
    };
}
