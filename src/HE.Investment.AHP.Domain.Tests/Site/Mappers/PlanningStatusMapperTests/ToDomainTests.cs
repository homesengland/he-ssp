using FluentAssertions;
using HE.Investment.AHP.Domain.Site.Mappers;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.Site.Mappers.PlanningStatusMapperTests;

public class ToDomainTests : TestBase<PlanningStatusMapper>
{
    [Theory]
    [InlineData(858110000, SitePlanningStatus.DetailedPlanningApprovalGranted)]
    [InlineData(858110001, SitePlanningStatus.DetailedPlanningApprovalGrantedWithFurtherSteps)]
    [InlineData(858110004, SitePlanningStatus.DetailedPlanningApplicationSubmitted)]
    [InlineData(858110002, SitePlanningStatus.OutlinePlanningApprovalGranted)]
    [InlineData(858110003, SitePlanningStatus.OutlinePlanningApplicationSubmitted)]
    [InlineData(858110005, SitePlanningStatus.PlanningDiscussionsUnderwayWithThePlanningOffice)]
    [InlineData(858110006, SitePlanningStatus.NoProgressOnPlanningApplication)]
    [InlineData(858110007, SitePlanningStatus.NoPlanningRequired)]
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
