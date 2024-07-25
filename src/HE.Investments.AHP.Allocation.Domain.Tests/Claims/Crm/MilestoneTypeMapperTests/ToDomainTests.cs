using FluentAssertions;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Claims.Crm;
using HE.Investments.Common.CRM.Model;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.Crm.MilestoneTypeMapperTests;

public class ToDomainTests : TestBase<MilestoneTypeMapper>
{
    [Theory]
    [InlineData((int)invln_Milestone.Acquisition, MilestoneType.Acquisition)]
    [InlineData((int)invln_Milestone.SoS, MilestoneType.StartOnSite)]
    [InlineData((int)invln_Milestone.PC, MilestoneType.Completion)]
    public void ShouldMapMilestoneType_WhenTypeIs(int milestoneType, MilestoneType expectedMilestoneType)
    {
        // given && when
        var result = TestCandidate.ToDomain(milestoneType);

        // then
        result.Should().Be(expectedMilestoneType);
    }

    [Theory]
    [InlineData(0)]
    [InlineData((int)invln_Milestone.Planning)]
    public void ShouldReturnUndefined_WhenMilestoneTypeIsNotKnown(int milestoneType)
    {
        // given && when
        var result = TestCandidate.ToDomain(milestoneType);

        // then
        result.Should().Be(MilestoneType.Undefined);
    }
}
