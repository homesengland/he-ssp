using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class ProvideAcquisitionMilestoneDetailsCommandHandler : UpdateDeliveryPhaseCommandHandler<ProvideAcquisitionMilestoneDetailsCommand>
{
    public ProvideAcquisitionMilestoneDetailsCommandHandler(IDeliveryPhaseRepository repository, IConsortiumUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override Task<OperationResult> Update(IDeliveryPhaseEntity entity, ProvideAcquisitionMilestoneDetailsCommand request, CancellationToken cancellationToken)
    {
        var operationResult = OperationResult.New();

        var acquisitionDate = operationResult.Aggregate(() => AcquisitionDate.FromDateDetails(request.AcquisitionDate));
        var milestonePaymentDate = operationResult.Aggregate(() => MilestonePaymentDate.FromDateDetails(request.PaymentDate));
        var milestone = operationResult.AggregateNullable(() => AcquisitionMilestoneDetails.Create(acquisitionDate, milestonePaymentDate));

        operationResult.CheckErrors();

        var milestones = new DeliveryPhaseMilestones(
            entity.IsOnlyCompletionMilestone,
            milestone,
            entity.DeliveryPhaseMilestones.StartOnSiteMilestone,
            entity.DeliveryPhaseMilestones.CompletionMilestone);

        entity.ProvideDeliveryPhaseMilestones(milestones);

        return Task.FromResult(operationResult);
    }
}
