using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Policies;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class ProvideCompletionMilestoneDetailsCommandHandler : UpdateDeliveryPhaseCommandHandler<ProvideCompletionMilestoneDetailsCommand>
{
    private readonly IMilestoneDatesInProgrammeDateRangePolicy _programmeDateRangePolicy;

    public ProvideCompletionMilestoneDetailsCommandHandler(
        IDeliveryPhaseRepository repository,
        IMilestoneDatesInProgrammeDateRangePolicy programmeDateRangePolicy,
        IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
        _programmeDateRangePolicy = programmeDateRangePolicy;
    }

    protected override async Task<OperationResult> Update(
        IDeliveryPhaseEntity entity,
        ProvideCompletionMilestoneDetailsCommand request,
        CancellationToken cancellationToken)
    {
        var operationResult = OperationResult.New();

        var completionDate = operationResult.AggregateNullable(() =>
            CompletionDate.Create(request.CompletionDate.Day, request.CompletionDate.Month, request.CompletionDate.Year));
        var milestonePaymentDate = operationResult.AggregateNullable(() =>
            MilestonePaymentDate.Create(request.PaymentDate.Day, request.PaymentDate.Month, request.PaymentDate.Year));
        var milestone = operationResult.AggregateNullable(() =>
            CompletionMilestoneDetails.Create(completionDate, milestonePaymentDate));

        operationResult.CheckErrors();

        var milestones = new DeliveryPhaseMilestones(
            entity.Organisation,
            entity.BuildActivity,
            entity.DeliveryPhaseMilestones.AcquisitionMilestone,
            entity.DeliveryPhaseMilestones.StartOnSiteMilestone,
            milestone);

        await entity.ProvideDeliveryPhaseMilestones(milestones, _programmeDateRangePolicy, cancellationToken);

        return operationResult;
    }
}
