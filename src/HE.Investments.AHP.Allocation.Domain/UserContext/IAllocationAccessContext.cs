using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investments.AHP.Allocation.Domain.UserContext;

public interface IAllocationAccessContext : IConsortiumAccessContext
{
    Task<bool> CanEditClaim();

    Task<bool> CanSubmitClaim();
}
