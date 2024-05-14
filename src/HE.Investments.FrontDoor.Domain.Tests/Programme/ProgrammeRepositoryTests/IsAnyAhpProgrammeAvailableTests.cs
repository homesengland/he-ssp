using FluentAssertions;
using HE.Investments.FrontDoor.Domain.Tests.Programme.TestObjectBuilders;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Programme.ProgrammeRepositoryTests;

public class IsAnyAhpProgrammeAvailableTests
{
    [Fact]
    public async Task ShouldReturnTrue_WhenThereIsAvailableProgramme()
    {
        // given
        var expectedStartDate = DateOnly.FromDateTime(DateTime.Now);

        var programmeRepository = ProgrammeRepositoryTestBuilder
            .New()
            .ReturnIsAnyAhpProgrammeAvailableResponse(expectedStartDate, true)
            .Build();

        // when
        var result = await programmeRepository.IsAnyAhpProgrammeAvailable(expectedStartDate, CancellationToken.None);

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldReturnFalse_WhenThereIsNotAnyAvailableProgramme()
    {
        // given
        var expectedStartDate = DateOnly.FromDateTime(DateTime.Now);

        var programmeRepository = ProgrammeRepositoryTestBuilder
            .New()
            .ReturnIsAnyAhpProgrammeAvailableResponse(expectedStartDate, false)
            .Build();

        // when
        var result = await programmeRepository.IsAnyAhpProgrammeAvailable(expectedStartDate, CancellationToken.None);

        // then
        result.Should().BeFalse();
    }
}
