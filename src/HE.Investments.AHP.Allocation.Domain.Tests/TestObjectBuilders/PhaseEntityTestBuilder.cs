using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Domain.Claims.Entities;
using HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;
using HE.Investments.Common.Tests.TestData;
using HE.Investments.Programme.Contract;
using HE.Investments.Programme.Contract.Enums;
using HE.Investments.TestsUtils.TestFramework;
using AllocationBasicInfo = HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects.AllocationBasicInfo;

namespace HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;

public class PhaseEntityTestBuilder : TestObjectBuilder<PhaseEntityTestBuilder, PhaseEntity>
{
    public PhaseEntityTestBuilder(PhaseEntity item)
        : base(item)
    {
    }

    protected override PhaseEntityTestBuilder Builder => this;

    public static PhaseEntityTestBuilder New() => new(new(
        PhaseId.From(GuidTestData.GuidTwo.ToString()),
        new AllocationBasicInfo(
            AllocationId.From(GuidTestData.GuidOne.ToString()),
            "Allocation",
            "G00001",
            "Reading",
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
            Tenure.AffordableRent),
        new PhaseName("Phase"),
        new NumberOfHomes(100),
        new BuildActivity(BuildActivityType.WorksOnly),
        MilestoneClaimTestBuilder.New().Build()));
}
