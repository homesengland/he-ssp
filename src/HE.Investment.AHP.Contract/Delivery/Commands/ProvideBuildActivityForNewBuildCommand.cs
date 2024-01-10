using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public record ProvideBuildActivityForNewBuildCommand(string ApplicationId, string DeliveryPhaseId, BuildActivityTypeForNewBuild? BuildActivityType)
    : IRequest<OperationResult>, IUpdateDeliveryPhaseCommand;
