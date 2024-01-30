using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Delivery.MilestonePayments.Commands;

public record ProvideStartOnSiteTrancheCommand(AhpApplicationId ApplicationId, DeliveryPhaseId DeliveryPhaseId, decimal? StartOnSiteTranche)
    : IRequest<OperationResult>, IUpdateDeliveryPhaseCommand;
