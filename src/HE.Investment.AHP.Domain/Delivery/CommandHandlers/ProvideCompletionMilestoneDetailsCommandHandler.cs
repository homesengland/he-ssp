using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class ProvideCompletionMilestoneDetailsCommandHandler : UpdateDeliveryPhaseCommandHandler<ProvideCompletionMilestoneDetailsCommand>
{
    public ProvideCompletionMilestoneDetailsCommandHandler(IDeliveryPhaseRepository repository, IConsortiumUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override Task<OperationResult> Update(
        IDeliveryPhaseEntity entity,
        ProvideCompletionMilestoneDetailsCommand request,
        CancellationToken cancellationToken)
    {
        var operationResult = OperationResult.New();

        var completionDate = operationResult.Aggregate(() => CompletionDate.FromDateDetails(request.CompletionDate));
        var milestonePaymentDate = operationResult.Aggregate(() => MilestonePaymentDate.FromDateDetails(request.PaymentDate));
        var milestone = operationResult.AggregateNullable(() => CompletionMilestoneDetails.Create(completionDate, milestonePaymentDate));

        operationResult.CheckErrors();

        var milestones = new DeliveryPhaseMilestones(
            entity.IsOnlyCompletionMilestone,
            entity.DeliveryPhaseMilestones.AcquisitionMilestone,
            entity.DeliveryPhaseMilestones.StartOnSiteMilestone,
            milestone);

        entity.ProvideDeliveryPhaseMilestones(milestones);

        return Task.FromResult(operationResult);
    }
}
