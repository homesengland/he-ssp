using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;

namespace HE.Investment.AHP.Domain.Delivery.Policies;

public interface IMilestoneDatesInProgrammeDateRangePolicy
{
    Task Validate(AhpApplicationId applicationId, DeliveryPhaseMilestones milestones, CancellationToken cancellationToken);
}
