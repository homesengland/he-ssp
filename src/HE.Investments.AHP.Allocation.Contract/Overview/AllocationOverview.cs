using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.Allocation.Contract.Overview;

public record AllocationOverview(
    AllocationBasicInfo BasicInfo,
    ModificationDetails ModificationDetails,
    bool IsInContract,
    string OrganisationName,
    string FdProjectId,
    bool IsDraft);
