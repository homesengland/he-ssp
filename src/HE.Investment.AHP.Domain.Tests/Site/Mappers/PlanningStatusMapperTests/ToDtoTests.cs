using FluentAssertions;
using HE.Investment.AHP.Domain.Site.Mappers;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.Site.Mappers.PlanningStatusMapperTests;

public class ToDtoTests : TestBase<PlanningStatusMapper>
{
    [Theory]
    [InlineData(SitePlanningStatus.DetailedPlanningApprovalGranted, 858110000)]
    [InlineData(SitePlanningStatus.DetailedPlanningApprovalGrantedWithFurtherSteps, 858110001)]
    [InlineData(SitePlanningStatus.DetailedPlanningApplicationSubmitted, 858110004)]
    [InlineData(SitePlanningStatus.OutlinePlanningApprovalGranted, 858110002)]
    [InlineData(SitePlanningStatus.OutlinePlanningApplicationSubmitted, 858110003)]
    [InlineData(SitePlanningStatus.PlanningDiscussionsUnderwayWithThePlanningOffice, 858110005)]
    [InlineData(SitePlanningStatus.NoProgressOnPlanningApplication, 858110006)]
    [InlineData(SitePlanningStatus.NoPlanningRequired, 858110007)]
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
