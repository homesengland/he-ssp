namespace HE.Investment.AHP.Contract.Delivery.Commands;

public interface IUpdateDeliveryPhaseCommand
{
    string ApplicationId { get; }

    string DeliveryPhaseId { get; }
}
