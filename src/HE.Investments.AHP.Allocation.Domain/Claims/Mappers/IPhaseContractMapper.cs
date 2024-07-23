using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Domain.Claims.Entities;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Mappers;

public interface IPhaseContractMapper
{
    Phase Map(PhaseEntity phase);
}
