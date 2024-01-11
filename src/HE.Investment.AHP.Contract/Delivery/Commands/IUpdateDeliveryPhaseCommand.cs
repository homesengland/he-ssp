using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public interface IUpdateDeliveryPhaseCommand
{
    AhpApplicationId ApplicationId { get; }

    string DeliveryPhaseId { get; }
}
