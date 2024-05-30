using FluentAssertions;
using HE.Investments.FrontDoor.Domain.Services;
using HE.Investments.FrontDoor.Domain.Tests.Services.TestDataBuilders;
using HE.Investments.Programme.Contract.Enums;
using HE.Investments.Programme.Contract.Queries;
using HE.Investments.TestsUtils.TestFramework;
using MediatR;
using Moq;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Services.ProgrammeAvailabilityServiceTests;

public class IsStartDateValidForAnyProgrammeTests : TestBase<ProgrammeAvailabilityService>
{
    [Fact]
    public async Task ShouldReturnTrue_WhenExpectedDateIsBeforeProgrammeEndDateAndProgrammeStartOnSiteEndDate()
    {
        // given
        var expectedDate = new DateOnly(2024, 5, 5);
        var programme = new ProgrammeBuilder()
            .WithEndDate(expectedDate.AddDays(1))
            .WithStartOnSiteEndDate(expectedDate.AddDays(1))
            .Build();
        var mediator = CreateAndRegisterDependencyMock<IMediator>();

        ProgrammeSettingsTestBuilder
            .New()
            .ReturnProgrammeId()
            .BuildMockAndRegister(this);

        mediator.Setup(x => x.Send(new GetProgrammeQuery(programme.Id), CancellationToken.None))
            .ReturnsAsync(programme);

        // when
        var result = await TestCandidate.IsStartDateValidForProgramme(programme.Id, expectedDate, CancellationToken.None);

        // then
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(1, -1)]
    [InlineData(-1, 1)]
    public async Task ShouldReturnFalse_WhenExpectedDateIsAfterStartOnSiteEndDate(int endDateDiff, int endSiteDateDiff)
    {
        // given
        var expectedDate = new DateOnly(2030, 12, 31);
        var programme = new ProgrammeBuilder()
            .WithEndDate(expectedDate.AddDays(endDateDiff))
            .WithStartOnSiteEndDate(expectedDate.AddDays(endSiteDateDiff))
            .Build();
        var mediator = CreateAndRegisterDependencyMock<IMediator>();

        ProgrammeSettingsTestBuilder
            .New()
            .ReturnProgrammeId()
            .BuildMockAndRegister(this);

        mediator.Setup(x => x.Send(new GetProgrammeQuery(programme.Id), CancellationToken.None))
            .ReturnsAsync(programme);

        // when
        var result = await TestCandidate.IsStartDateValidForProgramme(programme.Id, expectedDate, CancellationToken.None);

        // then
        result.Should().BeFalse();
    }
}
