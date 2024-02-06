using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.Delivery.MilestonePayments.Commands;
using HE.Investment.AHP.Domain.Delivery.CommandHandlers;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;

namespace HE.Investment.AHP.Domain.Delivery.Tranches.CommandHandlers;

public class ClaimMilestonesCommandHandler : UpdateDeliveryPhaseCommandHandler<ClaimMilestonesCommand>
{
    public ClaimMilestonesCommandHandler(IDeliveryPhaseRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override Task<OperationResult> Update(IDeliveryPhaseEntity entity, ClaimMilestonesCommand request, CancellationToken cancellationToken)
    {
        var deliveryPhaseTranches = entity.Tranches;
        deliveryPhaseTranches.ClaimMilestones(MilestoneFramework.Default, request.YesNo);
        return Task.FromResult(OperationResult.Success());
    }
}
