using HE.Investments.FrontDoor.Domain.Project;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.TestsUtils.TestFramework;
using ProjectGeographicFocus = HE.Investments.FrontDoor.Shared.Project.Contract.ProjectGeographicFocus;

namespace HE.Investments.FrontDoor.Domain.Tests.Project.TestDataBuilders;

public class ProjectEntityBuilder : TestObjectBuilder<ProjectEntityBuilder, ProjectEntity>
{
    private ProjectEntityBuilder()
        : base(new ProjectEntity(new FrontDoorProjectId("test-id-123"), new ProjectName("Test project")))
    {
    }

    protected override ProjectEntityBuilder Builder => this;

    public static ProjectEntityBuilder New() => new();

    public ProjectEntityBuilder WithSupportActivities(IList<SupportActivityType> supportActivityTypes) => SetProperty(x => x.SupportActivities, new SupportActivities(supportActivityTypes));

    public ProjectEntityBuilder WithRequiredFunding(bool isFundingRequired, RequiredFundingOption requiredFunding)
    {
        _ = SetProperty(x => x.IsFundingRequired, new IsFundingRequired(isFundingRequired));
        return SetProperty(x => x.RequiredFunding, new RequiredFunding(requiredFunding));
    }

    public ProjectEntityBuilder WithIsProfit(bool isProfit) => SetProperty(x => x.IsProfit, new IsProfit(isProfit));

    public ProjectEntityBuilder WithAffordableHomesAmount(AffordableHomesAmount affordableHomesAmount) => SetProperty(x => x.AffordableHomesAmount, new ProjectAffordableHomesAmount(affordableHomesAmount));

    public ProjectEntityBuilder WithOrganisationHomesBuilt(int organisationHomesBuilt) => SetProperty(x => x.OrganisationHomesBuilt, new OrganisationHomesBuilt(organisationHomesBuilt));

    public ProjectEntityBuilder WithInfrastructureType(IList<InfrastructureType> infrastructureTypes) => SetProperty(x => x.Infrastructure, new ProjectInfrastructure(infrastructureTypes));

    public ProjectEntityBuilder WithGeographicFocus(ProjectGeographicFocus geographicFocus) => SetProperty(x => x.GeographicFocus, new Domain.Project.ValueObjects.ProjectGeographicFocus(geographicFocus));

    public ProjectEntityBuilder WithIsSiteIdentified(bool isSiteIdentified) => SetProperty(x => x.IsSiteIdentified, new IsSiteIdentified(isSiteIdentified));

    public ProjectEntityBuilder WithIsSupportRequired(bool isSupportRequired) => SetProperty(x => x.IsSupportRequired, new IsSupportRequired(isSupportRequired));

    public ProjectEntityBuilder WithRegions(IList<RegionType> regionTypes) => SetProperty(x => x.Regions, new Regions(regionTypes));

    public ProjectEntityBuilder WithHomesNumber(int homesNumber) => SetProperty(x => x.HomesNumber, new HomesNumber(homesNumber));

    public ProjectEntityBuilder WithExpectedStartDate(string expectedStartMonth, string expectedStartYear) => SetProperty(x => x.ExpectedStartDate, new ExpectedStartDate(expectedStartMonth, expectedStartYear));

    public ProjectEntityBuilder WithNonSiteQuestionFulfilled()
    {
        return WithIsSiteIdentified(false)
            .WithGeographicFocus(ProjectGeographicFocus.Regional)
            .WithRegions(new List<RegionType> { RegionType.EastMidlands })
            .WithHomesNumber(35);
    }
}
