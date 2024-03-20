using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.FrontDoor.Domain.Site;
using HE.Investments.FrontDoor.Domain.Site.ValueObjects;
using HE.Investments.FrontDoor.Domain.Tests.Project.TestDataBuilders;
using HE.Investments.FrontDoor.Domain.Tests.Site.TestDataBuilders;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Site.ProjectSitesEntityTests;

public class RemoveAllProjectSitesTests : TestBase<ProjectSiteEntity>
{
    [Fact]
    public async Task ShouldRemoveSite_WhenProjectHadOnlyOneSite()
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
        await projectSites.RemoveAllProjectSites(removeSiteRepository.Object, userAccount, CancellationToken.None);

        // then
        removeSiteRepository.Verify(
            repo => repo.Remove(
                siteId,
                userAccount,
                CancellationToken.None),
            Times.Once);
    }

    [Fact]
    public async Task ShouldRemoveAllSites_WhenProjectHadMoreSites()
    {
        // given
        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;
        var firstSiteId = FrontDoorSiteIdTestData.IdOne;
        var secondSiteId = FrontDoorSiteIdTestData.IdTwo;
        var thirdSiteId = FrontDoorSiteIdTestData.IdThree;
        var projectId = FrontDoorProjectIdTestData.IdOne;
        var projectSites = ProjectSitesEntityBuilder
            .New()
            .WithSite(new SiteName("FirstSite"), firstSiteId)
            .WithSite(new SiteName("SecondSite"), secondSiteId)
            .WithSite(new SiteName("ThirdSite"), thirdSiteId)
            .Build();

        var removeSiteRepository = SiteRepositoryTestBuilder
            .New()
            .ReturnProjectSites(projectId, userAccount, projectSites)
            .BuildIRemoveSiteRepositoryMockAndRegister(this);

        // when
        await projectSites.RemoveAllProjectSites(removeSiteRepository.Object, userAccount, CancellationToken.None);

        // then
        removeSiteRepository.Verify(
            repo => repo.Remove(
                firstSiteId,
                userAccount,
                CancellationToken.None),
            Times.Once);

        removeSiteRepository.Verify(
            repo => repo.Remove(
                secondSiteId,
                userAccount,
                CancellationToken.None),
            Times.Once);

        removeSiteRepository.Verify(
            repo => repo.Remove(
                thirdSiteId,
                userAccount,
                CancellationToken.None),
            Times.Once);
    }
}
