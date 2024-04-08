using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.Delivery;

public record ApplicationDeliveryPhases(ApplicationDetails Application, int UnusedHomeTypesCount, IList<DeliveryPhaseBasicDetails> DeliveryPhases);
