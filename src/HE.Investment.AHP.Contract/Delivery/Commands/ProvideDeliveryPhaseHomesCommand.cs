using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public record ProvideDeliveryPhaseHomesCommand(
        AhpApplicationId ApplicationId,
        DeliveryPhaseId DeliveryPhaseId,
        IDictionary<string, string?> HomesToDeliver)
    : IDeliveryCommand, IUpdateDeliveryPhaseCommand;
