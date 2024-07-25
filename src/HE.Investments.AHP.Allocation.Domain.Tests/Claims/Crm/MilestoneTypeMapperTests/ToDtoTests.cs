using FluentAssertions;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Claims.Crm;
using HE.Investments.Common.CRM.Model;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.Crm.MilestoneTypeMapperTests;

public class ToDtoTests : TestBase<MilestoneTypeMapper>
{
    [Theory]
    [InlineData(MilestoneType.Acquisition, (int)invln_Milestone.Acquisition)]
    [InlineData(MilestoneType.StartOnSite, (int)invln_Milestone.SoS)]
    [InlineData(MilestoneType.Completion, (int)invln_Milestone.PC)]
    public void ShouldMapMilestoneType_WhenTypeIs(MilestoneType milestoneType, int expectedMilestoneType)
    {
        // given && when
        var result = TestCandidate.ToDto(milestoneType);

        // then
        result.Should().Be(expectedMilestoneType);
    }

    [Fact]
    public void ShouldThrowException_WhenTypeUndefined()
    {
        // given && when
        var map = () => TestCandidate.ToDto(MilestoneType.Undefined);

        // then
        map.Should().Throw<ArgumentException>();
    }
}
