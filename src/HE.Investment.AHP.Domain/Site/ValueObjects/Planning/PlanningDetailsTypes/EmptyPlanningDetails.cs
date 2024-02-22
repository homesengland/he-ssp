using HE.Investment.AHP.Contract.Site;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;

public class EmptyPlanningDetails : PlanningDetails
{
    public override SitePlanningStatus? PlanningStatus => null;

    protected override IReadOnlyCollection<string> ActiveFields => Array.Empty<string>();
}
