using HE.Investments.Account.Shared;
using HE.Investments.AHP.Allocation.Contract.Claims.Commands;
using HE.Investments.AHP.Allocation.Domain.Claims.Repositories;
using HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;

namespace HE.Investments.AHP.Allocation.Domain.Claims.CommandHandlers;

internal sealed class ProvideClaimConfirmationCommandHandler : ProvideClaimDetailsBaseCommandHandler<ProvideClaimConfirmationCommand>
{
    public ProvideClaimConfirmationCommandHandler(IPhaseRepository phaseRepository, IAccountUserContext accountUserContext)
        : base(phaseRepository, accountUserContext)
    {
    }

    protected override MilestoneClaimBase Provide(ProvideClaimConfirmationCommand request, MilestoneClaimBase claim)
    {
        return claim.WithConfirmation(request.IsConfirmed);
    }
}
