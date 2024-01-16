using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public record ProvideCompletionMilestoneDetailsCommand(AhpApplicationId ApplicationId, DeliveryPhaseId DeliveryPhaseId, DateDetails CompletionDate, DateDetails PaymentDate) : IRequest<OperationResult>, IUpdateDeliveryPhaseCommand;
