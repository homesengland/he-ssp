using FluentAssertions;
using HE.Investments.AHP.Allocation.Domain.Claims.Crm;
using HE.Investments.AHP.Allocation.Domain.Claims.Enums;
using HE.Investments.Common.CRM.Model;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.Crm.MilestoneStatusMapperTests;

public class ToDtoTests : TestBase<MilestoneStatusMapper>
{
    [Theory]
    [InlineData(MilestoneStatus.Draft, (int)invln_ClaimExternalStatus.Draft)]
    [InlineData(MilestoneStatus.Submitted, (int)invln_ClaimExternalStatus.Submitted)]
    [InlineData(MilestoneStatus.UnderReview, (int)invln_ClaimExternalStatus.UnderReview)]
    [InlineData(MilestoneStatus.Approved, (int)invln_ClaimExternalStatus.Approved)]
    [InlineData(MilestoneStatus.Rejected, (int)invln_ClaimExternalStatus.Rejected)]
    [InlineData(MilestoneStatus.Paid, (int)invln_ClaimExternalStatus.Paid)]
    public void ShouldMapMilestoneType_WhenTypeIs(MilestoneStatus milestoneStatus, int expectedMilestoneStatus)
    {
        // given && when
        var result = TestCandidate.ToDto(milestoneStatus);

        // then
        result.Should().Be(expectedMilestoneStatus);
    }

    [Fact]
    public void ShouldThrowException_WhenTypeUndefined()
    {
        // given && when
        var map = () => TestCandidate.ToDto(MilestoneStatus.Undefined);

        // then
        map.Should().Throw<ArgumentException>();
    }
}
