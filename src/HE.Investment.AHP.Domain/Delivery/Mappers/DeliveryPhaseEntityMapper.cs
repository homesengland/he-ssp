using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Domain.Common.Mappers;
using HE.Investment.AHP.Domain.Delivery.Entities;

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
            DateValueObjectMapper.ToContract(deliveryPhase.DeliveryPhaseMilestones.AcquisitionMilestone?.PaymentDate),
            DateValueObjectMapper.ToContract(deliveryPhase.DeliveryPhaseMilestones.StartOnSiteMilestone?.PaymentDate),
            DateValueObjectMapper.ToContract(deliveryPhase.DeliveryPhaseMilestones.CompletionMilestone?.PaymentDate));
    }
}
