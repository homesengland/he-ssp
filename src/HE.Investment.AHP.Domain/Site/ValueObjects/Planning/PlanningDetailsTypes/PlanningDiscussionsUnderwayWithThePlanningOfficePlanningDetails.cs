using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;

public class PlanningDiscussionsUnderwayWithThePlanningOfficePlanningDetails : PlanningDetails
{
    public PlanningDiscussionsUnderwayWithThePlanningOfficePlanningDetails(
        ExpectedPlanningApprovalDate? expectedPlanningApprovalDate = null,
        LandRegistryDetails? landRegistryDetails = null)
        : base(expectedPlanningApprovalDate: expectedPlanningApprovalDate, landRegistryDetails: landRegistryDetails)
    {
    }

    public override SitePlanningStatus? PlanningStatus => SitePlanningStatus.PlanningDiscussionsUnderwayWithThePlanningOffice;

    protected override IReadOnlyCollection<string> ActiveFields => new[]
    {
        nameof(ExpectedPlanningApprovalDate),
        nameof(LandRegistryDetails),
    };
}
