using HE.Investment.AHP.Domain.Delivery.ValueObjects;

namespace HE.Investment.AHP.Domain.Delivery.Policies;

public class MilestoneDatesInProgrammeDateRangePolicy : IMilestoneDatesInProgrammeDateRangePolicy
{
    public Task Validate(DeliveryPhaseMilestones milestones, CancellationToken cancellationToken)
    {
        // TODO: AB#98416 get current AHP programme and validate against dates
        return Task.CompletedTask;
    }
}
