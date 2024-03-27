using FluentAssertions;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.FrontDoor.Domain.Tests.Site.TestDataBuilders;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Site.ProjectSiteEntityTests;

public class CanBeCompletedTests
{
    [Fact]
    public void ShouldNotThrowDomainValidationException_WhenSiteCanBeCompleted()
    {
        // given
        var siteEntity = ProjectSiteEntityBuilder
            .New(null, null, null)
            .WithPlanningStatus(SitePlanningStatus.DetailedPlanningApplicationSubmitted)
            .WithHomesNumber(150)
            .WithLocalAuthority("E08000012", "Liverpool")
            .Build();

        // when
        var action = () => siteEntity.CanBeCompleted();

        // then
        action.Should().NotThrow<DomainValidationException>();
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenSiteIsMissingPlanningStatus()
    {
        // given
        var siteEntity = ProjectSiteEntityBuilder
            .New(null, null, null)
            .WithHomesNumber(150)
            .WithLocalAuthority("E08000012", "Liverpool")
            .Build();

        // when
        var action = () => siteEntity.CanBeCompleted();

        // then
        action.Should().Throw<DomainValidationException>().WithMessage(ValidationErrorMessage.ProvideAllSiteAnswers);
    }
}
