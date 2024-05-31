using HE.Investments.Programme.Contract;
using HE.Investments.Programme.Contract.Enums;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investments.FrontDoor.Domain.Tests.Services;

public class ProgrammeBuilder : TestObjectBuilder<ProgrammeBuilder, Investments.Programme.Contract.Programme>
{
    public ProgrammeBuilder()
        : base(new(
            new ProgrammeId("d5fe3baa-eeae-ee11-a569-0022480041cf"),
            "AHP 21-26 CME",
            "Affordable Homes Programme 2021-2026 Continuous Market Engagement",
            ProgrammeType.Ahp,
            new DateRange(new DateOnly(2021, 01, 01), new DateOnly(2026, 01, 01)),
            new DateRange(DateOnly.MinValue, DateOnly.MaxValue),
            new DateRange(new DateOnly(2024, 03, 01), new DateOnly(2025, 12, 01)),
            new DateRange(DateOnly.MinValue, DateOnly.MaxValue)))
    {
    }

    protected override ProgrammeBuilder Builder => this;

    public ProgrammeBuilder WithEndDate(DateOnly value) => SetProperty(x => x.ProgrammeDates.End, value);

    public ProgrammeBuilder WithStartOnSiteEndDate(DateOnly value) => SetProperty(x => x.StartOnSiteDates.End, value);
}
