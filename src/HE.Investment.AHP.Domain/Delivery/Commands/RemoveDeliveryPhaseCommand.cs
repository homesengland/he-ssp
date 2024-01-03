using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Delivery.Commands;

public record RemoveDeliveryPhaseCommand(
        string ApplicationId,
        string DeliveryPhaseId,
        RemoveDeliveryPhaseAnswer RemoveDeliveryPhaseAnswer)
    : IRequest<OperationResult>;
