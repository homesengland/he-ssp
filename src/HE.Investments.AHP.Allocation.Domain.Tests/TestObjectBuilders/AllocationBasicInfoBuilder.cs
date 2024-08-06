using HE.Investment.AHP.Contract.Application;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects;
using HE.Investments.Common.Tests.TestData;
using HE.Investments.Organisation.LocalAuthorities.ValueObjects;
using HE.Investments.Programme.Contract;
using HE.Investments.Programme.Contract.Enums;
using HE.Investments.TestsUtils.TestFramework;
using AllocationBasicInfo = HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects.AllocationBasicInfo;

namespace HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;

public class AllocationBasicInfoBuilder : TestObjectBuilder<AllocationBasicInfoBuilder, AllocationBasicInfo>
{
    public AllocationBasicInfoBuilder()
        : base(new AllocationBasicInfo(
            AllocationId.From(GuidTestData.GuidOne.ToString()),
            new AllocationName("Allocation"),
            new AllocationReferenceNumber("G00001"),
            new LocalAuthority(new LocalAuthorityCode("7000178"), "Oxford"),
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
            Tenure.AffordableRent,
            true))
    {
    }

    protected override AllocationBasicInfoBuilder Builder => this;
}
