using FluentAssertions;
using HE.Investments.Common.Utils;
using HE.Investments.Programme.Contract;
using HE.Investments.Programme.Domain.Entities;
using HE.Investments.Programme.Domain.Mappers;
using HE.Investments.Programme.Domain.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investments.Programme.Domain.Tests.Mappers;

public class ProgrammeMapperTests : TestBase<ProgrammeMapper>
{
    [Fact]
    public void ShouldMapProgramme()
    {
        // given
        var today = new DateOnly(2024, 06, 06);
        var programme = new ProgrammeEntity(
            ProgrammeId.From("61a40ef5-5054-445d-9d2d-4bc84260464e"),
            "Affordable Homes Programme 2021-2026 Continuous Market Engagement",
            new ProgrammeDates(today.AddDays(-10), today.AddDays(10)),
            new ProgrammeDates(null, null),
            new ProgrammeDates(null, null),
            new ProgrammeDates(null, null));

        MockDateTimeProvider(today);

        // when
        var result = TestCandidate.Map(programme);

        // then
        result.Id.Should().Be(programme.Id);
        result.Name.Should().Be(programme.Name);
        result.ShortName.Should().Be("AHP 21-26 CME");
        result.IsOpenForApplications.Should().BeTrue();
        result.ProgrammeDates.Should().Be(new DateRange(programme.ProgrammeDates.Start, programme.ProgrammeDates.End));
        result.FundingDates.Should().Be(new DateRange(DateOnly.MinValue, DateOnly.MaxValue));
        result.StartOnSiteDates.Should().Be(new DateRange(DateOnly.MinValue, DateOnly.MaxValue));
        result.CompletionDates.Should().Be(new DateRange(DateOnly.MinValue, DateOnly.MaxValue));
    }

    private void MockDateTimeProvider(DateOnly date)
    {
        var dateTimeProvider = CreateAndRegisterDependencyMock<IDateTimeProvider>();
        dateTimeProvider.Setup(x => x.Now)
            .Returns(date.ToDateTime(new TimeOnly(12, 23)));
    }
}
