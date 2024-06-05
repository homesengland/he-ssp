using HE.Investments.Programme.Contract;
using HE.Investments.Programme.Contract.Enums;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.Common.TestData;

internal sealed class ProgrammeBuilder : TestObjectBuilder<ProgrammeBuilder, Programme>
{
    public ProgrammeBuilder()
        : base(new(
            new ProgrammeId("d5fe3baa-eeae-ee11-a569-0022480041cf"),
            "AHP 21-26 CME",
            "Affordable Homes Programme 2021-2026 Continuous Market Engagement",
            true,
            ProgrammeType.Ahp,
            new DateRange(DateOnly.MinValue, DateOnly.MaxValue),
            new DateRange(DateOnly.MinValue, DateOnly.MaxValue),
            new DateRange(DateOnly.MinValue, DateOnly.MaxValue),
            new DateRange(DateOnly.MinValue, DateOnly.MaxValue)))
    {
    }

    protected override ProgrammeBuilder Builder => this;

    public ProgrammeBuilder WithFundingStartDate(DateTime value) =>
        SetProperty(x => x.FundingDates, Item.FundingDates with { Start = DateOnly.FromDateTime(value) });

    public ProgrammeBuilder WithFundingEndDate(DateTime value) =>
        SetProperty(x => x.FundingDates, Item.FundingDates with { End = DateOnly.FromDateTime(value) });

    public ProgrammeBuilder WithStartOnSiteStartDate(DateTime value) =>
        SetProperty(x => x.StartOnSiteDates, Item.StartOnSiteDates with { Start = DateOnly.FromDateTime(value) });

    public ProgrammeBuilder WithStartOnSiteEndDate(DateTime value) =>
        SetProperty(x => x.StartOnSiteDates, Item.StartOnSiteDates with { End = DateOnly.FromDateTime(value) });

    public ProgrammeBuilder WithCompletionStartDate(DateTime value) =>
        SetProperty(x => x.CompletionDates, Item.CompletionDates with { Start = DateOnly.FromDateTime(value) });

    public ProgrammeBuilder WithCompletionEndDate(DateTime value) =>
        SetProperty(x => x.CompletionDates, Item.CompletionDates with { End = DateOnly.FromDateTime(value) });
}
