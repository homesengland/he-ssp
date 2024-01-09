using HE.Investment.AHP.Contract.Delivery.Enums;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public record ProvideDeliveryPhaseNameCommand(
        string ApplicationId,
        string DeliveryPhaseId,
        string? DeliveryPhaseName)
    : IDeliveryCommand;
