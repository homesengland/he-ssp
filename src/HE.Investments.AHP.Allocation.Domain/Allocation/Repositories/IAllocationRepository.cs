using HE.Investments.Account.Shared.User;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Overview;
using HE.Investments.AHP.Allocation.Domain.Allocation.Entities;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investments.AHP.Allocation.Domain.Allocation.Repositories;

public interface IAllocationRepository
{
    Task<AllocationEntity> GetById(AllocationId id, UserAccount userAccount, CancellationToken cancellationToken);

    Task<AllocationOverview> GetOverview(AllocationId id, ConsortiumUserAccount userAccount, CancellationToken cancellationToken);
}
