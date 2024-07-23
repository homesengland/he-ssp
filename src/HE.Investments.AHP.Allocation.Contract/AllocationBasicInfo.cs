using HE.Investment.AHP.Contract.Application;

namespace HE.Investments.AHP.Allocation.Contract;

public record AllocationBasicInfo(
    AllocationId Id,
    string Name,
    string ReferenceNumber,
    string LocalAuthority,
    string ProgrammeName,
    Tenure Tenure) : IAllocation;
