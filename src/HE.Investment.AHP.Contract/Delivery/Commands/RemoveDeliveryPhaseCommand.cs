using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery.Enums;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public record RemoveDeliveryPhaseCommand(
        AhpApplicationId ApplicationId,
        string DeliveryPhaseId,
        RemoveDeliveryPhaseAnswer RemoveDeliveryPhaseAnswer)
    : IDeliveryCommand;
