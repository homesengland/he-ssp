using HE.Investment.AHP.Domain.Delivery.ValueObjects;

namespace HE.Investment.AHP.Domain.Delivery.Strategies;

public class MilestoneAvailabilityStrategy : IMilestoneAvailabilityStrategy
{
    public bool OnlyCompletionMilestone(bool isUnregisteredBody, BuildActivity buildActivity)
    {
        return isUnregisteredBody || buildActivity.IsOffTheShelfOrExistingSatisfactory;
    }
}
