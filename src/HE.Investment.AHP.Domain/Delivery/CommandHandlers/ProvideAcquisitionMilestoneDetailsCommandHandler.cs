using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Policies;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class ProvideAcquisitionMilestoneDetailsCommandHandler : UpdateDeliveryPhaseCommandHandler<ProvideAcquisitionMilestoneDetailsCommand>
{
    private readonly IMilestoneDatesInProgrammeDateRangePolicy _programmeDateRangePolicy;

    public ProvideAcquisitionMilestoneDetailsCommandHandler(
        IDeliveryPhaseRepository repository,
        IMilestoneDatesInProgrammeDateRangePolicy programmeDateRangePolicy,
        IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
        _programmeDateRangePolicy = programmeDateRangePolicy;
    }

    protected override async Task<OperationResult> Update(IDeliveryPhaseEntity entity, ProvideAcquisitionMilestoneDetailsCommand request, CancellationToken cancellationToken)
    {
        var operationResult = OperationResult.New();

        var acquisitionDate = operationResult.Aggregate(() =>
            AcquisitionDate.FromDateDetails(true, request.AcquisitionDate));
        var milestonePaymentDate = operationResult.Aggregate(() =>
            MilestonePaymentDate.FromDateDetails(true, request.PaymentDate));
        var milestone = operationResult.AggregateNullable(() => AcquisitionMilestoneDetails.Create(acquisitionDate, milestonePaymentDate));

        operationResult.CheckErrors();

        var milestones = new DeliveryPhaseMilestones(
            entity.IsOnlyCompletionMilestone,
            milestone,
            entity.DeliveryPhaseMilestones.StartOnSiteMilestone,
            entity.DeliveryPhaseMilestones.CompletionMilestone);

        await entity.ProvideDeliveryPhaseMilestones(milestones, _programmeDateRangePolicy, cancellationToken);

        return operationResult;
    }
}
