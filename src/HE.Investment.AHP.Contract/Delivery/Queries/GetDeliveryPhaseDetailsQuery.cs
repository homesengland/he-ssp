using HE.Investment.AHP.Contract.Application;
using MediatR;

namespace HE.Investment.AHP.Contract.Delivery.Queries;

public record GetDeliveryPhaseDetailsQuery(AhpApplicationId ApplicationId, DeliveryPhaseId DeliveryPhaseId, bool IncludeSummary = false) : IRequest<DeliveryPhaseDetails>;
