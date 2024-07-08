using HE.Investments.AHP.Allocation.Contract.Claims;

namespace HE.Investments.AHP.Allocation.Contract;

public record AllocationDetails(AllocationBasicInfo AllocationBasicInfo, List<Phase> PhaseList);
