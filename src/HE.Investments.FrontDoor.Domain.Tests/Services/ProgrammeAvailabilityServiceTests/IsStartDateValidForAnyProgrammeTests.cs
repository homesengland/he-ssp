using FluentAssertions;
using HE.Investments.FrontDoor.Domain.Services;
using HE.Investments.FrontDoor.Domain.Tests.Programme.TestObjectBuilders;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Services.ProgrammeAvailabilityServiceTests;

public class IsStartDateValidForAnyProgrammeTests
{
    [Fact]
    public async Task ShouldReturnTrue_WhenExpectedDateIsValidForAnyProgramme()
    {
        // given
        var programmeRepository = ProgrammeRepositoryTestBuilder
            .New()
            .ReturnProgrammes()
            .Build();

        var expectedDate = new DateOnly(2024, 5, 5);
        var service = new ProgrammeAvailabilityService();

        // when
        var result = service.IsStartDateValidForAnyProgramme(await programmeRepository.GetProgrammes(CancellationToken.None), expectedDate);

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldReturnFalse_WhenExpectedDateIsValidForAnyProgramme()
    {
        // given
        var programmeRepository = ProgrammeRepositoryTestBuilder
            .New()
            .ReturnProgrammes()
            .Build();

        var expectedDate = new DateOnly(2030, 12, 31);
        var service = new ProgrammeAvailabilityService();

        // when
        var result = service.IsStartDateValidForAnyProgramme(await programmeRepository.GetProgrammes(CancellationToken.None), expectedDate);

        // then
        result.Should().BeFalse();
    }
}
