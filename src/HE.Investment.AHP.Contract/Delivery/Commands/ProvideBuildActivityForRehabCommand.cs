using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public record ProvideBuildActivityForRehabCommand(string ApplicationId, string DeliveryPhaseId, BuildActivityTypeForRehab? BuildActivityType)
    : IRequest<OperationResult>, IUpdateDeliveryPhaseCommand;
