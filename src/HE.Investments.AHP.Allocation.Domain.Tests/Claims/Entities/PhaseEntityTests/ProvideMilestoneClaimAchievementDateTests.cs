using FluentAssertions;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;
using HE.Investments.AHP.Allocation.Domain.Tests.TestData;
using HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;
using HE.Investments.Common.Utils;
using Moq;
using Xunit;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.Entities.PhaseEntityTests;

public class ProvideMilestoneClaimAchievementDateTests
{
    private readonly IDateTimeProvider _dateTimeProvider = Mock.Of<IDateTimeProvider>();

    [Fact]
    public void ShouldChangeMilestoneAndMarkAsModified_WhenDifferentAchievementDateWasProvided()
    {
        // given
        GetDateTimeProviderMock();
        var testCandidate = PhaseEntityTestBuilder.New()
            .WithAcquisitionMilestone(MilestoneClaimTestBuilder.Draft()
                .WithType(MilestoneType.Acquisition)
                .WithMilestoneAchievedDate(new AchievementDate(new DateTime(2022, 5, 5, 0, 0, 0, DateTimeKind.Local)))
                .Build())
            .Build();
        var newAchievementDate = new AchievementDate(new DateTime(2023, 10, 10, 0, 0, 0, DateTimeKind.Local));

        // when
        testCandidate.ProvideMilestoneClaimAchievementDate(
            testCandidate.AcquisitionMilestone!,
            ProgrammeTestData.AhpCmeProgramme,
            newAchievementDate,
            _dateTimeProvider.Now);

        // then
        testCandidate.AcquisitionMilestone!.ClaimDate.AchievementDate.Should().Be(newAchievementDate);
        testCandidate.IsModified.Should().BeTrue();
    }

    [Fact]
    public void ShouldDoNothing_WhenTheSameAchievementDateWasProvided()
    {
        // given
        GetDateTimeProviderMock();
        var achievementDate = new AchievementDate(new DateTime(2023, 10, 10, 0, 0, 0, DateTimeKind.Local));
        var testCandidate = PhaseEntityTestBuilder.New()
            .WithAcquisitionMilestone(MilestoneClaimTestBuilder.Draft().WithType(MilestoneType.Acquisition).WithMilestoneAchievedDate(achievementDate).WithMilestoneSubmissionDate(null).Build())
            .Build();

        // when
        testCandidate.ProvideMilestoneClaimAchievementDate(
            testCandidate.AcquisitionMilestone!,
            ProgrammeTestData.AhpCmeProgramme,
            achievementDate,
            _dateTimeProvider.Now);

        // then
        testCandidate.AcquisitionMilestone!.ClaimDate.AchievementDate.Should().Be(achievementDate);
        testCandidate.IsModified.Should().BeFalse();
    }

    private void GetDateTimeProviderMock()
    {
        Mock.Get(_dateTimeProvider)
            .Setup(x => x.Now)
            .Returns(new DateTime(2024, 07, 07, 12, 25, 0, DateTimeKind.Unspecified));
    }
}
