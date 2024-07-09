using HE.Investments.Account.Shared.User;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Domain.Claims.Entities;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Repositories;

public interface IPhaseRepository
{
    Task<PhaseEntity> GetById(PhaseId phaseId, AllocationId allocationId, UserAccount userAccount, CancellationToken cancellationToken);

    Task Save(PhaseEntity phaseEntity, UserAccount userAccount, CancellationToken cancellationToken);
}
