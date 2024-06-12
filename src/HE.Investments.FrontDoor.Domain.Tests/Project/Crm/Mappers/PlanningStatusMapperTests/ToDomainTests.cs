using FluentAssertions;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.FrontDoor.Domain.Project.Storage.Crm.Mappers;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Project.Crm.Mappers.PlanningStatusMapperTests;

public class ToDomainTests : TestBase<PlanningStatusMapper>
{
    [Theory]
    [InlineData(134370000, SitePlanningStatus.DetailedPlanningApprovalGranted)]
    [InlineData(134370001, SitePlanningStatus.DetailedPlanningApprovalGrantedWithFurtherSteps)]
    [InlineData(134370002, SitePlanningStatus.DetailedPlanningApplicationSubmitted)]
    [InlineData(134370003, SitePlanningStatus.OutlinePlanningApprovalGranted)]
    [InlineData(134370004, SitePlanningStatus.OutlinePlanningApplicationSubmitted)]
    [InlineData(134370005, SitePlanningStatus.PlanningDiscussionsUnderwayWithThePlanningOffice)]
    [InlineData(134370006, SitePlanningStatus.NoProgressOnPlanningApplication)]
    [InlineData(134370007, SitePlanningStatus.NoPlanningRequired)]
    public void ShouldMapPlanningStatus_WhenCorrectValueIsProvided(int planningStatus, SitePlanningStatus expectedStatus)
    {
        // given & when
        var result = TestCandidate.ToDomain(planningStatus);

        // then
        result.Should().Be(expectedStatus);
    }

    [Fact]
    public void ShouldReturnUndefined_WhenUnknownValueIsProvided()
    {
        // given & when
        var result = TestCandidate.ToDomain(134370009);

        // then
        result.Should().Be(SitePlanningStatus.Undefined);
    }
}
