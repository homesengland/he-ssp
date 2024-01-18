using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Policies;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class ProvideStartOnSiteMilestoneDetailsCommandHandler : UpdateDeliveryPhaseCommandHandler<ProvideStartOnSiteMilestoneDetailsCommand>
{
    private readonly IMilestoneDatesInProgrammeDateRangePolicy _programmeDateRangePolicy;

    public ProvideStartOnSiteMilestoneDetailsCommandHandler(
        IDeliveryPhaseRepository repository,
        IMilestoneDatesInProgrammeDateRangePolicy programmeDateRangePolicy,
        IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
        _programmeDateRangePolicy = programmeDateRangePolicy;
    }

    protected override async Task<OperationResult> Update(IDeliveryPhaseEntity entity, ProvideStartOnSiteMilestoneDetailsCommand request)
    {
        var operationResult = OperationResult.New();

        var startOnSiteDate = operationResult.AggregateNullable(() =>
            StartOnSiteDate.Create(request.StartOnSiteDate.Day, request.StartOnSiteDate.Month, request.StartOnSiteDate.Year));
        var milestonePaymentDate = operationResult.AggregateNullable(() =>
            MilestonePaymentDate.Create(request.PaymentDate.Day, request.PaymentDate.Month, request.PaymentDate.Year));
        var milestone = operationResult.AggregateNullable(() =>
            StartOnSiteMilestoneDetails.Create(startOnSiteDate, milestonePaymentDate));

        operationResult.CheckErrors();

        var milestones = new DeliveryPhaseMilestones(
            entity.Organisation,
            entity.BuildActivity,
            entity.DeliveryPhaseMilestones.AcquisitionMilestone,
            milestone,
            entity.DeliveryPhaseMilestones.CompletionMilestone);

        await entity.ProvideDeliveryPhaseMilestones(milestones, _programmeDateRangePolicy);

        return operationResult;
    }
}
