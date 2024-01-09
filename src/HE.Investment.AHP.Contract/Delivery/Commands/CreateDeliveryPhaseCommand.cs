using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public record CreateDeliveryPhaseCommand(string ApplicationId, string DeliveryPhaseName) : IRequest<OperationResult<DeliveryPhaseId?>>;
