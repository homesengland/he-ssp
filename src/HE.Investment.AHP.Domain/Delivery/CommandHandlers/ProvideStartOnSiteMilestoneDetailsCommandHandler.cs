using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class ProvideStartOnSiteMilestoneDetailsCommandHandler : UpdateDeliveryPhaseCommandHandler<ProvideStartOnSiteMilestoneDetailsCommand>
{
    public ProvideStartOnSiteMilestoneDetailsCommandHandler(IDeliveryPhaseRepository repository, IConsortiumUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override Task<OperationResult> Update(
        IDeliveryPhaseEntity entity,
        ProvideStartOnSiteMilestoneDetailsCommand request,
        CancellationToken cancellationToken)
    {
        var operationResult = OperationResult.New();

        var startOnSiteDate = operationResult.Aggregate(() => StartOnSiteDate.FromDateDetails(request.StartOnSiteDate));
        var milestonePaymentDate = operationResult.Aggregate(() => MilestonePaymentDate.FromDateDetails(request.PaymentDate));
        var milestone = operationResult.AggregateNullable(() => StartOnSiteMilestoneDetails.Create(startOnSiteDate, milestonePaymentDate));

        operationResult.CheckErrors();

        var milestones = new DeliveryPhaseMilestones(
            entity.IsOnlyCompletionMilestone,
            entity.DeliveryPhaseMilestones.AcquisitionMilestone,
            milestone,
            entity.DeliveryPhaseMilestones.CompletionMilestone);

        entity.ProvideDeliveryPhaseMilestones(milestones);

        return Task.FromResult(operationResult);
    }
}
