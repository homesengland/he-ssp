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
            DateHelper.ToDateOnlyString(deliveryPhase.AcquisitionMilestone?.AcquisitionDate?.Value),
            DateHelper.ToDateOnlyString(deliveryPhase.StartOnSiteMilestone?.StartOnSiteDate?.Value),
            DateHelper.ToDateOnlyString(deliveryPhase.CompletionMilestone?.CompletionDate?.Value));
    }

    public static DeliveryPhaseDetails ToDeliveryPhaseDetails(IDeliveryPhaseEntity deliveryPhase)
    {
        return new DeliveryPhaseDetails(
            deliveryPhase.Application.Name.Name,
            deliveryPhase.Id.Value,
            deliveryPhase.Name.Value,
            deliveryPhase.TypeOfHomes,
            deliveryPhase.TotalHomesToBeDeliveredInThisPhase,
            deliveryPhase.Organisation.IsUnregisteredBody,
            MapDate(deliveryPhase.AcquisitionMilestone?.AcquisitionDate),
            MapDate(deliveryPhase.AcquisitionMilestone?.PaymentDate),
            MapDate(deliveryPhase.StartOnSiteMilestone?.StartOnSiteDate),
            MapDate(deliveryPhase.StartOnSiteMilestone?.PaymentDate),
            MapDate(deliveryPhase.CompletionMilestone?.CompletionDate),
            MapDate(deliveryPhase.CompletionMilestone?.PaymentDate),
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
