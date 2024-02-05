namespace HE.Investment.AHP.Contract.Delivery;

public record ApplicationDeliveryPhases(string ApplicationName, int UnusedHomeTypesCount, IList<DeliveryPhaseBasicDetails> DeliveryPhases, bool IsReadOnly);
