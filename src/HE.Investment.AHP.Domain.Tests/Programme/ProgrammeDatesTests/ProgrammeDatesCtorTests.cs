using FluentAssertions;
using HE.Investment.AHP.Domain.Programme;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Programme.ProgrammeDatesTests;

public class ProgrammeDatesCtorTests
{
    [Fact]
    public void ShouldThrowException_WhenStartAtIsAfterEndAt()
    {
        // given & when
        var create = () => new ProgrammeDates(
            new DateOnly(2022, 1, 2),
            new DateOnly(2022, 1, 1));

        // then
        create.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void ShouldCreateEntity_WhenStartAtIsBeforeEndAt()
    {
        // given & when
        var result = new ProgrammeDates(
            new DateOnly(2023, 10, 5),
            new DateOnly(2024, 10, 5));

        // then
        result.ProgrammeStartDate.Should().Be(new DateOnly(2023, 10, 05));
        result.ProgrammeEndDate.Should().Be(new DateOnly(2024, 10, 05));
    }
}
