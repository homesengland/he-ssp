using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public record ProvideDeliveryPhaseNameCommand(
        AhpApplicationId ApplicationId,
        string DeliveryPhaseId,
        string? DeliveryPhaseName)
    : IDeliveryCommand;
