using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public record CreateDeliveryPhaseCommand(AhpApplicationId ApplicationId, string DeliveryPhaseName) : IRequest<OperationResult<DeliveryPhaseId?>>;
