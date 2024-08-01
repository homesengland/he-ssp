using HE.Investment.AHP.Domain.Delivery.ValueObjects;

namespace HE.Investment.AHP.Domain.Delivery.Policies;

public class OnlyCompletionMilestonePolicy : IOnlyCompletionMilestonePolicy
{
    public bool Validate(bool isUnregisteredBody, BuildActivity buildActivity)
    {
        return isUnregisteredBody || buildActivity.IsOffTheShelfOrExistingSatisfactory;
    }
}
