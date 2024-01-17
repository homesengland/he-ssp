using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public record UnCompleteDeliveryPhaseCommand(
        AhpApplicationId ApplicationId,
        DeliveryPhaseId DeliveryPhaseId)
    : IDeliveryCommand, IUpdateDeliveryPhaseCommand;
