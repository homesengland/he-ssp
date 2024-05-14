using FluentAssertions;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.Common.Utils;
using HE.Investments.FrontDoor.Domain.Project;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Domain.Services.Strategies;
using HE.Investments.FrontDoor.Domain.Site;
using HE.Investments.FrontDoor.Domain.Site.ValueObjects;
using HE.Investments.FrontDoor.Domain.Tests.Project.TestDataBuilders;
using HE.Investments.FrontDoor.Domain.Tests.Site.TestDataBuilders;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Services.StrategiesTests;

public class LoanApplicationConversionStrategyTests
{
    private readonly LoanApplicationConversionStrategy _strategy = new();

    [Fact]
    public void ShouldReturnLoansApplicationType_WhenProjectAndSitesAreValidForLoanApplication()
    {
        // given
        var project = CreateLoansValidProjectEntity();

        var projectSites = CreateLoansValidProjectSites();

        // when
        var result = _strategy.Apply(project, projectSites);

        // then
        result.Should().Be(ApplicationType.Loans);
    }

    [Fact]
    public void ShouldReturnUndefinedApplicationType_WhenSitesAreValidButProjectIsNotValidForLoanApplication()
    {
        // given
        var project = CreateLoansValidProjectEntity();
        project.ProvideOrganisationHomesBuilt(new OrganisationHomesBuilt(5000));

        var projectSites = CreateLoansValidProjectSites();

        // when
        var result = _strategy.Apply(project, projectSites);

        // then
        result.Should().Be(ApplicationType.Undefined);
    }

    [Fact]
    public void ShouldReturnUndefinedApplicationType_WhenProjectIsValidButSitesAreNotValidForLoanApplication()
    {
        // given
        var project = CreateLoansValidProjectEntity();

        var projectSites = CreateLoansValidProjectSites();

        var siteTwo = ProjectSiteEntityBuilder
            .New(new SiteName("second test name"), new FrontDoorProjectId("second project id"), new FrontDoorSiteId("second site id"))
            .WithPlanningStatus(SitePlanningStatus.DetailedPlanningApplicationSubmitted)
            .WithHomesNumber(150)
            .WithLocalAuthority("E08000012", "Liverpool")
            .Build();
        projectSites.Sites.Add(siteTwo);

        // when
        var result = _strategy.Apply(project, projectSites);

        // then
        result.Should().Be(ApplicationType.Undefined);
    }

    private ProjectEntity CreateLoansValidProjectEntity()
    {
        var dateTimeProviderMock = DateTimeProviderBuilder.New().ReturnDate(new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Build();
        DateTimeUtil.SetDateTimeProvider(dateTimeProviderMock);
        return ProjectEntityBuilder
            .New()
            .WithSupportActivities([SupportActivityType.DevelopingHomes])
            .WithAffordableHomesAmount(AffordableHomesAmount.OnlyOpenMarketHomes)
            .WithOrganisationHomesBuilt(1000)
            .WithIsSiteIdentified(true)
            .WithIsSupportRequired(true)
            .WithRequiredFunding(true, RequiredFundingOption.Between1MlnAnd5Mln)
            .WithIsProfit(true)
            .WithExpectedStartDate("01", "2022")
            .Build();
    }

    private ProjectSitesEntity CreateLoansValidProjectSites()
    {
        var site = ProjectSiteEntityBuilder
            .New(new SiteName("first test name"), new FrontDoorProjectId("first project id"), new FrontDoorSiteId("first site id"))
            .WithPlanningStatus(SitePlanningStatus.DetailedPlanningApplicationSubmitted)
            .WithHomesNumber(150)
            .WithLocalAuthority("E08000012", "Liverpool")
            .Build();

        return ProjectSitesEntityBuilder
            .New()
            .AddSite(site)
            .Build();
    }
}
