using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;

public class EmptyPlanningDetails : PlanningDetails, IQuestion
{
    public override SitePlanningStatus? PlanningStatus => null;

    public override bool IsAnswered()
    {
        return false;
    }
}
