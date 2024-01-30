using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;

public class NoPlanningRequiredPlanningDetails : PlanningDetails, IQuestion
{
    public NoPlanningRequiredPlanningDetails(LandRegistryDetails? landRegistryDetails = null)
        : base(landRegistryDetails: landRegistryDetails)
    {
    }

    public override SitePlanningStatus? PlanningStatus => SitePlanningStatus.NoPlanningRequired;

    public override bool IsAnswered()
    {
        return LandRegistryDetails != null && LandRegistryDetails.IsAnswered();
    }
}
