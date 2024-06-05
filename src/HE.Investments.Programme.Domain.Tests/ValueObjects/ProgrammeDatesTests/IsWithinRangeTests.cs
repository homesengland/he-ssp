using FluentAssertions;
using HE.Investments.Programme.Domain.ValueObjects;

namespace HE.Investments.Programme.Domain.Tests.ValueObjects.ProgrammeDatesTests;

public class IsWithinRangeTests
{
    [Theory]
    [InlineData(2021, 12, 31)]
    [InlineData(2023, 01, 01)]
    public void ShouldReturnFalse_WhenDateIsOutsideProgrammeDates(int year, int month, int day)
    {
        // given
        var programmeDates = new ProgrammeDates(new DateOnly(2022, 1, 1), new DateOnly(2022, 12, 31));
        var date = new DateOnly(year, month, day);

        // when
        var result = programmeDates.IsWithinRange(date);

        // then
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(2022, 01, 01)]
    [InlineData(2022, 06, 06)]
    [InlineData(2022, 12, 31)]
    public void ShouldReturnTrue_WhenDateIsWithinProgrammeDates(int year, int month, int day)
    {
        // given
        var programmeDates = new ProgrammeDates(new DateOnly(2022, 1, 1), new DateOnly(2022, 12, 31));
        var date = new DateOnly(year, month, day);

        // when
        var result = programmeDates.IsWithinRange(date);

        // then
        result.Should().BeTrue();
    }
}
