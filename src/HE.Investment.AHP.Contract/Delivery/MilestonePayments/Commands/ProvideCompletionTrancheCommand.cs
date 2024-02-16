using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Delivery.MilestonePayments.Commands;

public record ProvideCompletionTrancheCommand(AhpApplicationId ApplicationId, DeliveryPhaseId DeliveryPhaseId, string? CompletionTranche)
    : IRequest<OperationResult>, IUpdateDeliveryPhaseCommand;
