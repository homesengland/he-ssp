using HE.Investments.Common.Contract;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public record ProvideCompletionMilestoneDetailsCommand(string ApplicationId, string DeliveryPhaseId, DateDetails CompletionDate, DateDetails PaymentDate) : IRequest<OperationResult>, IUpdateDeliveryPhaseCommand;
