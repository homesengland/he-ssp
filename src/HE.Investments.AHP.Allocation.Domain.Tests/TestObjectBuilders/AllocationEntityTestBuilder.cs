using HE.Investment.AHP.Contract.Application;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Domain.Allocation.Entities;
using HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects;
using HE.Investments.AHP.Allocation.Domain.Claims.Entities;
using HE.Investments.Common.Contract;
using HE.Investments.Programme.Contract;
using HE.Investments.Programme.Contract.Enums;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;

public class AllocationEntityTestBuilder : TestObjectBuilder<AllocationEntityTestBuilder, AllocationEntity>
{
    public AllocationEntityTestBuilder(AllocationEntity item)
        : base(item)
    {
    }

    protected override AllocationEntityTestBuilder Builder => this;

    public static AllocationEntityTestBuilder New() => new(new AllocationEntity(
        new AllocationId("00000000-0000-1111-1111-111111111111"),
        new AllocationName("Important Allocation"),
        new AllocationReferenceNumber("G0000013"),
        new LocalAuthority("Code", "Reading"),
        new Programme.Contract.Programme(
            ProgrammeId.From("d5fe3baa-eeae-ee11-a569-0022480041cf"),
            "Ahp",
            "ProgrammeType Ahp",
            true,
            ProgrammeType.Ahp,
            new DateRange(new DateOnly(2021, 1, 1), new DateOnly(2022, 1, 1)),
            new DateRange(new DateOnly(2021, 1, 1), new DateOnly(2022, 1, 1)),
            new DateRange(new DateOnly(2021, 1, 1), new DateOnly(2022, 1, 1)),
            new DateRange(new DateOnly(2021, 1, 1), new DateOnly(2022, 1, 1))),
        new AllocationTenure(Tenure.AffordableRent),
        new GrantDetails(1000, 500, 500),
        []));

    public AllocationEntityTestBuilder WithListOfPhaseClaims(List<PhaseEntity> value) => SetProperty(x => x.ListOfPhaseClaims, value);
}
