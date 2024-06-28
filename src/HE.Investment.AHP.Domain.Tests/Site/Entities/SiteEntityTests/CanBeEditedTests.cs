using FluentAssertions;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Tests.Application.TestDataBuilders;
using HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Tests.Site.Entities.SiteEntityTests;

public class CanBeEditedTests
{
    [Fact]
    public void ShouldReturnTrue_WhenAllApplicationsAreDraftOrReferredBackToApplicant()
    {
        // given
        var applications = new List<ApplicationBasicDetails>
        {
            ApplicationBasicDetailsBuilder.New().WithName("First application").WithStatus(ApplicationStatus.Draft).Build(),
            ApplicationBasicDetailsBuilder.New().WithName("Second application").WithStatus(ApplicationStatus.ReferredBackToApplicant).Build(),
        };
        var siteEntity = SiteEntityBuilder.New().WithStatus(SiteStatus.Submitted).Build();

        // when
        var result = siteEntity.CanBeEdited(applications);

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalse_WhenAnyApplicationIsNotDraftOrReferredBackToApplicant()
    {
        // given
        var applications = new List<ApplicationBasicDetails>
        {
            ApplicationBasicDetailsBuilder.New().WithName("First application").WithStatus(ApplicationStatus.Draft).Build(),
            ApplicationBasicDetailsBuilder.New().WithName("Second application").WithStatus(ApplicationStatus.ApplicationSubmitted).Build(),
        };
        var siteEntity = SiteEntityBuilder.New().WithStatus(SiteStatus.Submitted).Build();

        // when
        var result = siteEntity.CanBeEdited(applications);

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnTrue_WhenApplicationsListIsEmpty()
    {
        // given
        var applications = new List<ApplicationBasicDetails>();
        var siteEntity = SiteEntityBuilder.New().WithStatus(SiteStatus.Submitted).Build();

        // when
        var result = siteEntity.CanBeEdited(applications);

        // then
        result.Should().BeTrue();
    }
}
