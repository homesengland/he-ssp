using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public record ProvideDeliveryPhaseNameCommand(
        AhpApplicationId ApplicationId,
        DeliveryPhaseId DeliveryPhaseId,
        string? DeliveryPhaseName)
    : IDeliveryCommand;
