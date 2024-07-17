using HE.Investments.Account.Shared;
using HE.Investments.AHP.Allocation.Contract.Claims.Commands;
using HE.Investments.AHP.Allocation.Domain.Claims.Repositories;
using HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;

namespace HE.Investments.AHP.Allocation.Domain.Claims.CommandHandlers;

internal sealed class ProvideCostsIncurredCommandHandler :
    ProvideClaimDetailsBaseCommandHandler<ProvideCostsIncurredCommand>
{
    public ProvideCostsIncurredCommandHandler(IPhaseRepository phaseRepository, IAccountUserContext accountUserContext)
        : base(phaseRepository, accountUserContext)
    {
    }

    protected override MilestoneClaim Provide(ProvideCostsIncurredCommand request, MilestoneClaim claim)
    {
        return claim.WithCostsIncurred(request.CostsIncurred);
    }
}
