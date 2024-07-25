using FluentAssertions;
using HE.Investments.AHP.Allocation.Domain.Claims.Crm;
using HE.Investments.AHP.Allocation.Domain.Claims.Enums;
using HE.Investments.Common.CRM.Model;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.Crm.MilestoneStatusMapperTests;

public class ToDomainTests : TestBase<MilestoneStatusMapper>
{
    [Theory]
    [InlineData((int)invln_ClaimExternalStatus.Draft, MilestoneStatus.Draft)]
    [InlineData((int)invln_ClaimExternalStatus.Submitted, MilestoneStatus.Submitted)]
    [InlineData((int)invln_ClaimExternalStatus.UnderReview, MilestoneStatus.UnderReview)]
    [InlineData((int)invln_ClaimExternalStatus.Approved, MilestoneStatus.Approved)]
    [InlineData((int)invln_ClaimExternalStatus.Rejected, MilestoneStatus.Rejected)]
    [InlineData((int)invln_ClaimExternalStatus.Paid, MilestoneStatus.Paid)]
    public void ShouldMapMilestoneStatus_WhenStatusIs(int milestoneStatus, MilestoneStatus expectedMilestoneStatus)
    {
        // given && when
        var result = TestCandidate.ToDomain(milestoneStatus);

        // then
        result.Should().Be(expectedMilestoneStatus);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(858110010)]
    public void ShouldReturnUndefined_WhenMilestoneStatusIsNotKnown(int milestoneStatus)
    {
        // given && when
        var result = TestCandidate.ToDomain(milestoneStatus);

        // then
        result.Should().Be(MilestoneStatus.Undefined);
    }
}
