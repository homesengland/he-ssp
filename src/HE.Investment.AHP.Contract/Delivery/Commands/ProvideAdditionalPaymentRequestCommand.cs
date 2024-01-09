using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public record ProvideAdditionalPaymentRequestCommand(string ApplicationId, string DeliveryPhaseId, bool? IsAdditionalPaymentRequested) : IRequest<OperationResult>, IUpdateDeliveryPhaseCommand;
