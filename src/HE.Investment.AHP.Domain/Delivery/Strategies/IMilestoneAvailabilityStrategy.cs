using HE.Investment.AHP.Domain.Delivery.ValueObjects;

namespace HE.Investment.AHP.Domain.Delivery.Strategies;

public interface IMilestoneAvailabilityStrategy
{
    bool OnlyCompletionMilestone(bool isUnregisteredBody, BuildActivity buildActivity);
}
