using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects;
using HE.Investments.AHP.Allocation.Domain.Claims.Entities;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;
using AhpProgramme = HE.Investments.Programme.Contract.Programme;

namespace HE.Investments.AHP.Allocation.Domain.Allocation.Entities;

public class AllocationEntity : DomainEntity
{
    public AllocationEntity(
        AllocationId id,
        AllocationName name,
        AllocationReferenceNumber referenceNumber,
        LocalAuthority localAuthority,
        AhpProgramme programme,
        AllocationTenure tenure,
        GrantDetails grantDetails,
        List<PhaseEntity> listOfPhaseClaims)
    {
        Id = id;
        Name = name;
        ReferenceNumber = referenceNumber;
        LocalAuthority = localAuthority;
        Programme = programme;
        Tenure = tenure;
        GrantDetails = grantDetails;
        ListOfPhaseClaims = listOfPhaseClaims;
    }

    public AllocationId Id { get; }

    public AllocationName Name { get; }

    public AllocationReferenceNumber ReferenceNumber { get; }

    public LocalAuthority LocalAuthority { get; }

    public AhpProgramme Programme { get; }

    public AllocationTenure Tenure { get; }

    public GrantDetails GrantDetails { get; }

    public List<PhaseEntity> ListOfPhaseClaims { get; }

    public static AllocationEntity New(
        AllocationId id,
        AllocationName name,
        AllocationReferenceNumber referenceNumber,
        LocalAuthority localAuthority,
        AhpProgramme programme,
        AllocationTenure tenure,
        GrantDetails grantDetails,
        List<PhaseEntity> listOfPhaseClaims)
    {
        return new AllocationEntity(
            id,
            name,
            referenceNumber,
            localAuthority,
            programme,
            tenure,
            grantDetails,
            listOfPhaseClaims);
    }
}
