using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investments.Common.WWW.Helpers;

namespace HE.Investment.AHP.Domain.Delivery.Mappers;

public static class DeliveryPhaseEntityMapper
{
    public static DeliveryPhaseDetails ToContract(IDeliveryPhaseEntity deliveryPhase)
    {
        return new DeliveryPhaseDetails(
            deliveryPhase.Application.Name.Name,
            deliveryPhase.Id.Value,
            deliveryPhase.Name.Value,
            deliveryPhase.TypeOfHomes,
            deliveryPhase.TotalHomesToBeDeliveredInThisPhase,
            DateHelper.ToDateOnlyString(deliveryPhase.AcquisitionMilestone?.AcquisitionDate?.Date),
            DateHelper.ToDateOnlyString(deliveryPhase.StartOnSiteMilestone?.StartOnSiteDate?.Date),
            DateHelper.ToDateOnlyString(deliveryPhase.CompletionMilestone?.CompletionDate?.Date));
    }
}
