using HE.Investment.AHP.Contract.Delivery.MilestonePayments.Commands;
using HE.Investment.AHP.Domain.Delivery.CommandHandlers;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;

namespace HE.Investment.AHP.Domain.Delivery.Tranches.CommandHandlers;

public class ProvideStartOnSiteTrancheCommandHandler : UpdateDeliveryPhaseCommandHandler<ProvideStartOnSiteTrancheCommand>
{
    public ProvideStartOnSiteTrancheCommandHandler(IDeliveryPhaseRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override Task<OperationResult> Update(IDeliveryPhaseEntity entity, ProvideStartOnSiteTrancheCommand request, CancellationToken cancellationToken)
    {
        var deliveryPhaseTranches = entity.GetTranches();
        deliveryPhaseTranches.ProvideStartOnSiteTranche(request.StartOnSiteTranche);
        return Task.FromResult(OperationResult.Success());
    }
}
