using FluentAssertions;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.FrontDoor.Domain.Tests.Site.TestDataBuilders;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Site.ProjectSiteEntityTests;

public class IsSiteValidForLoanApplicationTests
{
    [Fact]
    public void ShouldReturnFalse_WhenSiteIsNotValidForLoanApplication()
    {
        // given
        var site = ProjectSiteEntityBuilder
            .New(null, null, null)
            .WithPlanningStatus(SitePlanningStatus.DetailedPlanningApplicationSubmitted)
            .WithHomesNumber(150)
            .WithLocalAuthority("E08000003", "Manchester")
            .Build();

        // when
        var result = site.IsSiteValidForLoanApplication();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnTrue_WhenSiteIsValidForLoanApplication()
    {
        // given
        var site = ProjectSiteEntityBuilder
            .New(null, null, null)
            .WithPlanningStatus(SitePlanningStatus.DetailedPlanningApplicationSubmitted)
            .WithHomesNumber(150)
            .WithLocalAuthority("E08000012", "Liverpool")
            .Build();

        // when
        var result = site.IsSiteValidForLoanApplication();

        // then
        result.Should().BeTrue();
    }
}
