using HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects;
using HE.Investments.AHP.Allocation.Domain.Claims.Entities;
using HE.Investments.Common.Domain;
using AllocationBasicInfo = HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects.AllocationBasicInfo;

namespace HE.Investments.AHP.Allocation.Domain.Allocation.Entities;

public sealed class AllocationEntity : DomainEntity
{
    private readonly IList<PhaseEntity> _phases;

    public AllocationEntity(
        AllocationBasicInfo allocationBasicInfo,
        GrantDetails grantDetails,
        IEnumerable<PhaseEntity> listOfPhaseClaims)
    {
        BasicInfo = allocationBasicInfo;
        GrantDetails = grantDetails;
        _phases = listOfPhaseClaims.ToList();
    }

    public AllocationBasicInfo BasicInfo { get; }

    public GrantDetails GrantDetails { get; }

    public IEnumerable<PhaseEntity> ListOfPhaseClaims => _phases;
}
