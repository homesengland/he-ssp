using MediatR;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public record CreateDeliveryPhaseCommand(string ApplicationId, string DeliveryPhaseName) : IRequest<DeliveryPhaseId>;
