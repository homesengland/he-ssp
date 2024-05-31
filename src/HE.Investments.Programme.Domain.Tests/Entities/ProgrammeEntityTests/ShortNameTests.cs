using FluentAssertions;
using HE.Investments.Programme.Contract;
using HE.Investments.Programme.Domain.Entities;
using HE.Investments.Programme.Domain.ValueObjects;

namespace HE.Investments.Programme.Domain.Tests.Entities.ProgrammeEntityTests;

public class ShortNameTests
{
    [Theory]
    [InlineData("Affordable Homes Programme 2021-2026 Continuous Market Engagement", "AHP 21-26 CME")]
    [InlineData("Affordable Homes Programme 2025-2032 Single Homelessness Accommodation", "AHP 25-32 SHA")]
    public void ShouldParseShortName_WhenCreatingProgramme(string name, string shortName)
    {
        // given & when
        var programme = new ProgrammeEntity(
            ProgrammeId.From("61a40ef5-5054-445d-9d2d-4bc84260464e"),
            name,
            new ProgrammeDates(null, null),
            new ProgrammeDates(null, null),
            new ProgrammeDates(null, null),
            new ProgrammeDates(null, null));

        // then
        programme.ShortName.Should().Be(shortName);
    }
}
