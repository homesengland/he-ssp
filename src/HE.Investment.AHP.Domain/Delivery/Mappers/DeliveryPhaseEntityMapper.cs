using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Domain.Common.Mappers;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Delivery.Mappers;

public static class DeliveryPhaseEntityMapper
{
    public static DeliveryPhaseBasicDetails ToDeliveryPhaseBasicDetails(IDeliveryPhaseEntity deliveryPhase)
    {
        return new DeliveryPhaseBasicDetails(
            deliveryPhase.Application.Name.Value,
            deliveryPhase.Id.Value,
            deliveryPhase.Name.Value,
            deliveryPhase.TotalHomesToBeDeliveredInThisPhase,
            DateDetails.FromDateTime(deliveryPhase.DeliveryPhaseMilestones.AcquisitionMilestone?.PaymentDate?.Value),
            DateDetails.FromDateTime(deliveryPhase.DeliveryPhaseMilestones.StartOnSiteMilestone?.PaymentDate?.Value),
            DateDetails.FromDateTime(deliveryPhase.DeliveryPhaseMilestones.CompletionMilestone?.PaymentDate?.Value));
    }
}
