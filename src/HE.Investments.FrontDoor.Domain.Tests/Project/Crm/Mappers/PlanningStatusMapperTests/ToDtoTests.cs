using FluentAssertions;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Project.Crm.Mappers.PlanningStatusMapperTests;

public class ToDtoTests : TestBase<PlanningStatusMapper>
{
    [Theory]
    [InlineData(SitePlanningStatus.DetailedPlanningApprovalGranted, 134370000)]
    [InlineData(SitePlanningStatus.DetailedPlanningApprovalGrantedWithFurtherSteps, 134370001)]
    [InlineData(SitePlanningStatus.DetailedPlanningApplicationSubmitted, 134370002)]
    [InlineData(SitePlanningStatus.OutlinePlanningApprovalGranted, 134370003)]
    [InlineData(SitePlanningStatus.OutlinePlanningApplicationSubmitted, 134370004)]
    [InlineData(SitePlanningStatus.PlanningDiscussionsUnderwayWithThePlanningOffice, 134370005)]
    [InlineData(SitePlanningStatus.NoProgressOnPlanningApplication, 134370006)]
    [InlineData(SitePlanningStatus.NoPlanningRequired, 134370007)]
    public void ShouldMapPlanningStatus_WhenCorrectValueIsProvided(SitePlanningStatus planningStatus, int expectedResult)
    {
        // given & when
        var result = TestCandidate.ToDto(planningStatus);

        // then
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData((int)SitePlanningStatus.Undefined)]
    [InlineData(int.MaxValue)]
    public void ShouldThrowException_WhenPlanningStatusIsOutsideValidRange(int planningStatus)
    {
        // given & when
        Action map = () => TestCandidate.ToDto((SitePlanningStatus)planningStatus);

        // then
        map.Should().Throw<ArgumentException>();
    }
}
