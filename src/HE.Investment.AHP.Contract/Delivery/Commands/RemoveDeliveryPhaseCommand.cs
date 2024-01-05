using HE.Investment.AHP.Contract.Delivery.Enums;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public record RemoveDeliveryPhaseCommand(
        string ApplicationId,
        string DeliveryPhaseId,
        RemoveDeliveryPhaseAnswer RemoveDeliveryPhaseAnswer)
    : DeliveryCommandBase(ApplicationId);
