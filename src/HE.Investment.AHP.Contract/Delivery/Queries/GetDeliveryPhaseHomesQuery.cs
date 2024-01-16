using HE.Investment.AHP.Contract.Application;
using MediatR;

namespace HE.Investment.AHP.Contract.Delivery.Queries;

public record GetDeliveryPhaseHomesQuery(AhpApplicationId ApplicationId, DeliveryPhaseId DeliveryPhaseId) : IRequest<DeliveryPhaseHomes>;
