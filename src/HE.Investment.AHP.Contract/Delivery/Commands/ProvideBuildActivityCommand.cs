using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public record ProvideBuildActivityCommand(AhpApplicationId ApplicationId, DeliveryPhaseId DeliveryPhaseId, BuildActivityType? BuildActivityType)
    : IRequest<OperationResult>, IUpdateDeliveryPhaseCommand;
