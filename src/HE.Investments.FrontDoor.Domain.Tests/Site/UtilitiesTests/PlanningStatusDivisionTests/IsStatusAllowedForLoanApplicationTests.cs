using FluentAssertions;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.FrontDoor.Domain.Site.Utilities;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Site.UtilitiesTests.PlanningStatusDivisionTests;

public class IsStatusAllowedForLoanApplicationTests
{
    [Theory]
    [InlineData(SitePlanningStatus.DetailedPlanningApprovalGranted)]
    [InlineData(SitePlanningStatus.DetailedPlanningApprovalGrantedWithFurtherSteps)]
    [InlineData(SitePlanningStatus.DetailedPlanningApplicationSubmitted)]
    [InlineData(SitePlanningStatus.OutlinePlanningApprovalGranted)]
    [InlineData(SitePlanningStatus.OutlinePlanningApplicationSubmitted)]
    [InlineData(SitePlanningStatus.PlanningDiscussionsUnderwayWithThePlanningOffice)]
    public void ShouldReturnTrue_WhenStatusIsAllowedForLoanApplication(SitePlanningStatus status)
    {
        // given && when
        var result = PlanningStatusDivision.IsStatusAllowedForLoanApplication(status);

        // then
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(SitePlanningStatus.Undefined)]
    [InlineData(SitePlanningStatus.NoProgressOnPlanningApplication)]
    [InlineData(SitePlanningStatus.NoPlanningRequired)]
    public void ShouldReturnFalse_WhenStatusIsNotAllowedForLoanApplication(SitePlanningStatus status)
    {
        // given && when
        var result = PlanningStatusDivision.IsStatusAllowedForLoanApplication(status);

        // then
        result.Should().BeFalse();
    }
}
