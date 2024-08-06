using HE.Investment.AHP.Contract.Application;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.Organisation.LocalAuthorities.ValueObjects;

namespace HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects;

public sealed record AllocationBasicInfo(
    AllocationId Id,
    AllocationName Name,
    AllocationReferenceNumber ReferenceNumber,
    LocalAuthority LocalAuthority,
    Programme.Contract.Programme Programme,
    Tenure Tenure,
    bool IsInContract);
