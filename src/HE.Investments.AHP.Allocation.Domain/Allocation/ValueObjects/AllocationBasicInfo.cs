using HE.Investment.AHP.Contract.Application;
using HE.Investments.AHP.Allocation.Contract;

namespace HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects;

public sealed record AllocationBasicInfo(
    AllocationId Id,
    string Name,
    string ReferenceNumber,
    string LocalAuthority,
    Programme.Contract.Programme Programme,
    Tenure Tenure);
