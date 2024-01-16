using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public record ProvideDeliveryPhaseHomesCommand(
    AhpApplicationId ApplicationId,
    DeliveryPhaseId DeliveryPhaseId)
    : IDeliveryCommand;

