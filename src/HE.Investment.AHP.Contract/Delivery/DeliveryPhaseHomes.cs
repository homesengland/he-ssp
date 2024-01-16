namespace HE.Investment.AHP.Contract.Delivery;

public record DeliveryPhaseHomes(
    DeliveryPhaseId DeliveryPhaseId,
    string DeliveryPhaseName,
    string ApplicationName,
    IList<HomeTypesToDeliver> HomeTypesToDeliver);
