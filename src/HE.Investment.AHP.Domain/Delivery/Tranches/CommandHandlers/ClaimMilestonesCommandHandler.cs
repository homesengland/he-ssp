using HE.Investment.AHP.Contract.Delivery.MilestonePayments.Commands;
using HE.Investment.AHP.Domain.Delivery.CommandHandlers;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investment.AHP.Domain.Delivery.Tranches.CommandHandlers;

public class ClaimMilestonesCommandHandler : UpdateDeliveryPhaseCommandHandler<ClaimMilestonesCommand>
{
    public ClaimMilestonesCommandHandler(IDeliveryPhaseRepository repository, IConsortiumUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override Task<OperationResult> Update(IDeliveryPhaseEntity entity, ClaimMilestonesCommand request, CancellationToken cancellationToken)
    {
        var deliveryPhaseTranches = entity.Tranches;
        deliveryPhaseTranches.ClaimMilestones(request.UnderstandClaimingMilestones);
        return Task.FromResult(OperationResult.Success());
    }
}
