using FluentAssertions;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.FrontDoor.Contract.Site.Enums;
using HE.Investments.FrontDoor.Domain.Site;
using HE.Investments.FrontDoor.Domain.Site.ValueObjects;
using HE.Investments.FrontDoor.Domain.Tests.Project.TestDataBuilders;
using HE.Investments.FrontDoor.Domain.Tests.Site.TestDataBuilders;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Site.ProjectSitesEntityTests;

public class RemoveSiteTests : TestBase<ProjectSiteEntity>
{
    [Fact]
    public async Task ShouldRemoveSite_WhenTheRemoveSiteAnswersIsYes()
    {
        // given
        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;
        var siteId = FrontDoorSiteIdTestData.IdTwo;
        var projectId = FrontDoorProjectIdTestData.IdOne;
        var projectSites = ProjectSitesEntityBuilder.New().WithSite(new SiteName("SiteName"), siteId).Build();

        var removeSiteRepository = SiteRepositoryTestBuilder
            .New()
            .ReturnProjectSites(projectId, userAccount, projectSites)
            .BuildIRemoveSiteRepositoryMockAndRegister(this);

        // when
        await projectSites.Remove(removeSiteRepository.Object, siteId, userAccount, RemoveSiteAnswer.Yes, CancellationToken.None);

        // then
        removeSiteRepository.Verify(
            repo => repo.Remove(
                siteId,
                userAccount,
                CancellationToken.None),
            Times.Once);
    }

    [Fact]
    public async Task ShouldNotRemoveSite_WhenTheRemoveSiteAnswersIsNo()
    {
        // given
        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;
        var siteId = FrontDoorSiteIdTestData.IdTwo;
        var projectId = FrontDoorProjectIdTestData.IdOne;
        var projectSites = ProjectSitesEntityBuilder.New().WithSite(new SiteName("SiteName"), siteId).Build();

        var removeSiteRepository = SiteRepositoryTestBuilder
            .New()
            .ReturnProjectSites(projectId, userAccount, projectSites)
            .BuildIRemoveSiteRepositoryMockAndRegister(this);

        // when
        await projectSites.Remove(removeSiteRepository.Object, siteId, userAccount, RemoveSiteAnswer.No, CancellationToken.None);

        // then
        removeSiteRepository.Verify(
            repo => repo.Remove(
                siteId,
                userAccount,
                CancellationToken.None),
            Times.Never);
    }

    [Fact]
    public async Task ShouldThrowDomainValidationException_WhenRemoveSiteAnswerIsNotProvided()
    {
        // given
        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;
        var siteId = FrontDoorSiteIdTestData.IdTwo;
        var projectId = FrontDoorProjectIdTestData.IdOne;
        var projectSites = ProjectSitesEntityBuilder.New().WithSite(new SiteName("SiteName"), siteId).Build();

        var removeSiteRepository = SiteRepositoryTestBuilder
            .New()
            .ReturnProjectSites(projectId, userAccount, projectSites)
            .BuildIRemoveSiteRepositoryMockAndRegister(this);

        // when
        var action = async () => await projectSites.Remove(removeSiteRepository.Object, siteId, userAccount, RemoveSiteAnswer.Undefined, CancellationToken.None);

        // then
        await action.Should().ThrowAsync<DomainValidationException>().WithMessage("Select yes if you want to remove this site");
    }
}
