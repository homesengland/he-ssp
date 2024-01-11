using HE.Investment.AHP.Contract.Application;
using MediatR;

namespace HE.Investment.AHP.Contract.Delivery.Queries;

public record GetDeliveryPhasesQuery(AhpApplicationId ApplicationId) : IRequest<ApplicationDeliveryPhases>;
