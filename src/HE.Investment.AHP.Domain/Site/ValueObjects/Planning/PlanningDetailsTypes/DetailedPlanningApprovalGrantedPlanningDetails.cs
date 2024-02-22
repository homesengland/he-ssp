using HE.Investment.AHP.Contract.Site;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;

public class DetailedPlanningApprovalGrantedPlanningDetails : PlanningDetails
{
    public DetailedPlanningApprovalGrantedPlanningDetails(
        ReferenceNumber? referenceNumber = null,
        DetailedPlanningApprovalDate? detailedPlanningApprovalDate = null,
        bool? isGrantFundingForAllHomes = null)
        : base(
            referenceNumber,
            detailedPlanningApprovalDate,
            isGrantFundingForAllHomesCoveredByApplication: isGrantFundingForAllHomes)
    {
    }

    public override SitePlanningStatus? PlanningStatus => SitePlanningStatus.DetailedPlanningApprovalGranted;

    protected override IReadOnlyCollection<string> ActiveFields => new[]
    {
        nameof(ReferenceNumber),
        nameof(DetailedPlanningApprovalDate),
        nameof(IsGrantFundingForAllHomesCoveredByApplication),
    };
}
