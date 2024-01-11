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
            deliveryPhase.Name?.Value,
            deliveryPhase.TotalHomesToBeDeliveredInThisPhase,
            DateHelper.ToDateOnlyString(deliveryPhase.DeliveryPhaseMilestones.AcquisitionMilestone?.MilestoneDate?.Value),
            DateHelper.ToDateOnlyString(deliveryPhase.DeliveryPhaseMilestones.StartOnSiteMilestone?.MilestoneDate?.Value),
            DateHelper.ToDateOnlyString(deliveryPhase.DeliveryPhaseMilestones.CompletionMilestone?.MilestoneDate?.Value));
    }

    public static DeliveryPhaseDetails ToDeliveryPhaseDetails(IDeliveryPhaseEntity deliveryPhase)
    {
        return new DeliveryPhaseDetails(
            deliveryPhase.Application.Name.Name,
            deliveryPhase.Id.Value,
            deliveryPhase.Name?.Value ?? string.Empty,
            deliveryPhase.TypeOfHomes,
            deliveryPhase.BuildActivityType.NewBuild,
            deliveryPhase.BuildActivityType.Rehab,
            deliveryPhase.TotalHomesToBeDeliveredInThisPhase,
            deliveryPhase.Organisation.IsUnregisteredBody,
            MapDate(deliveryPhase.DeliveryPhaseMilestones.AcquisitionMilestone?.MilestoneDate),
            MapDate(deliveryPhase.DeliveryPhaseMilestones.AcquisitionMilestone?.PaymentDate),
            MapDate(deliveryPhase.DeliveryPhaseMilestones.StartOnSiteMilestone?.MilestoneDate),
            MapDate(deliveryPhase.DeliveryPhaseMilestones.StartOnSiteMilestone?.PaymentDate),
            MapDate(deliveryPhase.DeliveryPhaseMilestones.CompletionMilestone?.MilestoneDate),
            MapDate(deliveryPhase.DeliveryPhaseMilestones.CompletionMilestone?.PaymentDate),
            deliveryPhase.IsAdditionalPaymentRequested?.IsRequested);
    }

    private static DateDetails? MapDate(DateValueObject? date)
    {
        return date != null
            ? new DateDetails(
                date.Value.Day.ToString(CultureInfo.InvariantCulture),
                date.Value.Month.ToString(CultureInfo.InvariantCulture),
                date.Value.Year.ToString(CultureInfo.InvariantCulture))
            : null;
    }
}
