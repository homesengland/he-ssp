using HE.Investment.AHP.Contract.Application;
using MediatR;

namespace HE.Investment.AHP.Contract.Delivery.Queries;

public record GetDeliveryPhaseDetailsQuery(AhpApplicationId ApplicationId, string DeliveryPhaseId) : IRequest<DeliveryPhaseDetails>;
