using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public record ProvideAdditionalPaymentRequestCommand(AhpApplicationId ApplicationId, DeliveryPhaseId DeliveryPhaseId, bool? IsAdditionalPaymentRequested) : IRequest<OperationResult>, IUpdateDeliveryPhaseCommand;
