using HE.Investments.Account.Shared.User;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Domain.Allocation.Entities;

namespace HE.Investments.AHP.Allocation.Domain.Allocation.Repositories;

public interface IAllocationRepository
{
    Task<AllocationEntity> GetById(AllocationId id, UserAccount userAccount, CancellationToken cancellationToken);
}
