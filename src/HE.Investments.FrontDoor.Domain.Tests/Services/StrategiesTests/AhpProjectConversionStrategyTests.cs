using FluentAssertions;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.Common.Utils;
using HE.Investments.FrontDoor.Domain.Project;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Domain.Services.Strategies;
using HE.Investments.FrontDoor.Domain.Site;
using HE.Investments.FrontDoor.Domain.Site.ValueObjects;
using HE.Investments.FrontDoor.Domain.Tests.Programme.TestObjectBuilders;
using HE.Investments.FrontDoor.Domain.Tests.Project.TestDataBuilders;
using HE.Investments.FrontDoor.Domain.Tests.Site.TestDataBuilders;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Services.StrategiesTests;

public class AhpProjectConversionStrategyTests
{
    [Fact]
    public async Task ShouldReturnAhpApplicationType_WhenProjectAndSitesAreValidForAhpProject()
    {
        // given
        var project = CreateAhpValidProjectEntity();

        var strategy = new AhpProjectConversionStrategy(
            ProgrammeRepositoryTestBuilder
                .New()
                .ReturnIsAnyAhpProgrammeAvailableResponse(project.ExpectedStartDate.Value, true)
                .Build());

        var projectSites = CreateAhpValidProjectSites();

        // when
        var result = await strategy.Apply(project, projectSites, CancellationToken.None);

        // then
        result.Should().Be(ApplicationType.Ahp);
    }

    [Fact]
    public async Task ShouldReturnUndefinedApplicationType_WhenSitesAreValidButProjectIsNotValidForAhpProject()
    {
        // given
        var project = CreateAhpValidProjectEntity();
        project.ProvideAffordableHomesAmount(new ProjectAffordableHomesAmount(AffordableHomesAmount.OnlyOpenMarketHomes));

        var strategy = new AhpProjectConversionStrategy(
            ProgrammeRepositoryTestBuilder
                .New()
                .ReturnIsAnyAhpProgrammeAvailableResponse(project.ExpectedStartDate.Value, true)
                .Build());

        var projectSites = CreateAhpValidProjectSites();

        // when
        var result = await strategy.Apply(project, projectSites, CancellationToken.None);

        // then
        result.Should().Be(ApplicationType.Undefined);
    }

    [Fact]
    public async Task ShouldReturnUndefinedApplicationType_WhenProjectIsValidButSitesAreNotValidForAhpProject()
    {
        // given
        var project = CreateAhpValidProjectEntity();

        var strategy = new AhpProjectConversionStrategy(
            ProgrammeRepositoryTestBuilder
                .New()
                .ReturnIsAnyAhpProgrammeAvailableResponse(project.ExpectedStartDate.Value, true)
                .Build());

        var projectSites = CreateAhpValidProjectSites();

        var newSite = ProjectSiteEntityBuilder
            .New(new SiteName("new site test name"), new FrontDoorProjectId("new project id"), new FrontDoorSiteId("new site id"))
            .WithPlanningStatus(SitePlanningStatus.DetailedPlanningApplicationSubmitted)
            .WithHomesNumber(50)
            .WithLocalAuthority("9000003", "Barnet")
            .Build();
        projectSites.Sites.Add(newSite);

        // when
        var result = await strategy.Apply(project, projectSites, CancellationToken.None);

        // then
        result.Should().Be(ApplicationType.Undefined);
    }

    private ProjectEntity CreateAhpValidProjectEntity()
    {
        var dateTimeProviderMock = DateTimeProviderBuilder.New().ReturnDate(new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Build();
        DateTimeUtil.SetDateTimeProvider(dateTimeProviderMock);
        return ProjectEntityBuilder
            .New()
            .WithSupportActivities([SupportActivityType.DevelopingHomes])
            .WithAffordableHomesAmount(AffordableHomesAmount.OnlyAffordableHomes)
            .WithOrganisationHomesBuilt(1000)
            .WithIsSiteIdentified(true)
            .WithIsSupportRequired(true)
            .WithRequiredFunding(true, RequiredFundingOption.Between1MlnAnd5Mln)
            .WithIsProfit(true)
            .WithExpectedStartDate("01", "2022")
            .Build();
    }

    private ProjectSitesEntity CreateAhpValidProjectSites()
    {
        var siteOne = ProjectSiteEntityBuilder
            .New(new SiteName("first test name"), new FrontDoorProjectId("first project id"), new FrontDoorSiteId("first site id"))
            .WithPlanningStatus(SitePlanningStatus.PlanningDiscussionsUnderwayWithThePlanningOffice)
            .WithHomesNumber(30)
            .WithLocalAuthority("E08000012", "Liverpool")
            .Build();

        var siteTwo = ProjectSiteEntityBuilder
            .New(new SiteName("second test name"), new FrontDoorProjectId("second project id"), new FrontDoorSiteId("second site id"))
            .WithPlanningStatus(SitePlanningStatus.DetailedPlanningApplicationSubmitted)
            .WithHomesNumber(450)
            .WithLocalAuthority("E08000012", "Liverpool")
            .Build();

        return ProjectSitesEntityBuilder
            .New()
            .AddSite(siteOne)
            .AddSite(siteTwo)
            .Build();
    }
}
