using System.Globalization;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.WWW.Helpers;

namespace HE.Investment.AHP.Domain.Delivery.Mappers;

public static class DeliveryPhaseEntityMapper
{
    public static DeliveryPhaseBasicDetails ToDeliveryPhaseBasicDetails(IDeliveryPhaseEntity deliveryPhase)
    {
        return new DeliveryPhaseBasicDetails(
            deliveryPhase.Application.Name.Name,
            deliveryPhase.Id.Value,
            deliveryPhase.Name.Value,
            deliveryPhase.TotalHomesToBeDeliveredInThisPhase,
            DateHelper.ToDateOnlyString(deliveryPhase.DeliveryPhaseMilestones.AcquisitionMilestone?.PaymentDate?.Value),
            DateHelper.ToDateOnlyString(deliveryPhase.DeliveryPhaseMilestones.StartOnSiteMilestone?.PaymentDate?.Value),
            DateHelper.ToDateOnlyString(deliveryPhase.DeliveryPhaseMilestones.CompletionMilestone?.PaymentDate?.Value));
    }

    public static DateDetails? MapDate(DateValueObject? date)
    {
        return date != null
            ? new DateDetails(
                date.Value.Day.ToString(CultureInfo.InvariantCulture),
                date.Value.Month.ToString(CultureInfo.InvariantCulture),
                date.Value.Year.ToString(CultureInfo.InvariantCulture))
            : null;
    }
}
