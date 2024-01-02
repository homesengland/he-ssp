using MediatR;

namespace HE.Investment.AHP.Contract.Delivery.Queries;

public record GetDeliveryPhasesQuery(string ApplicationId) : IRequest<ApplicationDeliveryPhases>;
