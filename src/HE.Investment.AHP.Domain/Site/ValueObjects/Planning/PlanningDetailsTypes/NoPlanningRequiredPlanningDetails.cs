using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;

public class NoPlanningRequiredPlanningDetails : PlanningDetails
{
    public NoPlanningRequiredPlanningDetails(LandRegistryDetails? landRegistryDetails = null)
        : base(landRegistryDetails: landRegistryDetails)
    {
    }

    public override SitePlanningStatus? PlanningStatus => SitePlanningStatus.NoPlanningRequired;

    protected override IReadOnlyCollection<string> ActiveFields => new[]
    {
        nameof(LandRegistryDetails),
    };
}
