using HE.Investment.AHP.Domain.Delivery.ValueObjects;

namespace HE.Investment.AHP.Domain.Delivery.Policies;

public interface IOnlyCompletionMilestonePolicy
{
    bool Validate(bool isUnregisteredBody, BuildActivity buildActivity);
}
