using HE.Investment.AHP.Contract.Delivery.MilestonePayments.Commands;
using HE.Investment.AHP.Domain.Delivery.CommandHandlers;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;

namespace HE.Investment.AHP.Domain.Delivery.Tranches.CommandHandlers;

public class ProvideCompletionTrancheCommandHandler : UpdateDeliveryPhaseCommandHandler<ProvideCompletionTrancheCommand>
{
    public ProvideCompletionTrancheCommandHandler(IDeliveryPhaseRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override Task<OperationResult> Update(IDeliveryPhaseEntity entity, ProvideCompletionTrancheCommand request, CancellationToken cancellationToken)
    {
        var deliveryPhaseTranches = entity.GetTranches();
        deliveryPhaseTranches.ProvideCompletionTranche(request.CompletionTranche);
        return Task.FromResult(OperationResult.Success());
    }
}
