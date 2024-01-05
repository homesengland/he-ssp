using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class ProvideStartOnSiteMilestoneDetailsCommandHandler : UpdateDeliveryPhaseCommandHandler<ProvideStartOnSiteMilestoneDetailsCommand>
{
    public ProvideStartOnSiteMilestoneDetailsCommandHandler(IDeliveryPhaseRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override Task<OperationResult> Update(IDeliveryPhaseEntity entity, ProvideStartOnSiteMilestoneDetailsCommand request)
    {
        var operationResult = OperationResult.New();

        var startOnSiteDate = operationResult.AggregateNullable(() =>
            StartOnSiteDate.Create(request.StartOnSiteDate.Day, request.StartOnSiteDate.Month, request.StartOnSiteDate.Year));
        var milestonePaymentDate = operationResult.AggregateNullable(() =>
            MilestonePaymentDate.Create(request.PaymentDate.Day, request.PaymentDate.Month, request.PaymentDate.Year));
        var details = operationResult.AggregateNullable(() => StartOnSiteMilestoneDetails.Create(startOnSiteDate, milestonePaymentDate));

        entity.ProvideStartOnSiteMilestoneDetails(details);

        return Task.FromResult(operationResult);
    }
}
