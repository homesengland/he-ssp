using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Delivery.Policies;

public interface IMilestoneDatesInProgrammeDateRangePolicy
{
    Task Validate(DeliveryPhaseMilestones milestones, CancellationToken cancellationToken);
}
