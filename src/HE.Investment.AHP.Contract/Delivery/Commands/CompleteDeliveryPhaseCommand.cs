using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public record CompleteDeliveryPhaseCommand(
        AhpApplicationId ApplicationId,
        DeliveryPhaseId DeliveryPhaseId,
        IsSectionCompleted? IsSectionCompleted)
    : IDeliveryCommand, IUpdateDeliveryPhaseCommand;
