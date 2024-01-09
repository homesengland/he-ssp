using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class ProvideCompletionMilestoneDetailsCommandHandler : UpdateDeliveryPhaseCommandHandler<ProvideCompletionMilestoneDetailsCommand>
{
    public ProvideCompletionMilestoneDetailsCommandHandler(IDeliveryPhaseRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override Task<OperationResult> Update(IDeliveryPhaseEntity entity, ProvideCompletionMilestoneDetailsCommand request)
    {
        var operationResult = OperationResult.New();

        var completionDate = operationResult.AggregateNullable(() =>
            CompletionDate.Create(request.CompletionDate.Day, request.CompletionDate.Month, request.CompletionDate.Year));
        var milestonePaymentDate = operationResult.AggregateNullable(() =>
            MilestonePaymentDate.Create(request.PaymentDate.Day, request.PaymentDate.Month, request.PaymentDate.Year));
        var details = operationResult.AggregateNullable(() => CompletionMilestoneDetails.Create(completionDate, milestonePaymentDate));

        entity.ProvideCompletionMilestoneDetails(details);

        return Task.FromResult(operationResult);
    }
}
