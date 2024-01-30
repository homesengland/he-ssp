using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;

public class NoProgressOnPlanningApplicationPlanningDetails : PlanningDetails, IQuestion
{
    public NoProgressOnPlanningApplicationPlanningDetails(
        ExpectedPlanningApprovalDate? expectedPlanningApprovalDate = null,
        LandRegistryDetails? landRegistryDetails = null)
        : base(expectedPlanningApprovalDate: expectedPlanningApprovalDate, landRegistryDetails: landRegistryDetails)
    {
    }

    public override SitePlanningStatus? PlanningStatus => SitePlanningStatus.NoProgressOnPlanningApplication;

    public override bool IsAnswered()
    {
        return ExpectedPlanningApprovalDate.IsProvided() && LandRegistryDetails != null && LandRegistryDetails.IsAnswered();
    }
}
