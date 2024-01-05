using MediatR;

namespace HE.Investment.AHP.Contract.Delivery.Queries;

public record GetDeliveryPhaseDetailsQuery(string ApplicationId, string DeliveryPhaseId) : IRequest<DeliveryPhaseDetails>;
