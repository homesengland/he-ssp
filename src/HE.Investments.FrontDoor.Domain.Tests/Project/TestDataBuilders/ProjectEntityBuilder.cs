using HE.Investments.FrontDoor.Domain.Project;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.TestsUtils.TestFramework;
using ProjectGeographicFocus = HE.Investments.FrontDoor.Shared.Project.Contract.ProjectGeographicFocus;

namespace HE.Investments.FrontDoor.Domain.Tests.Project.TestDataBuilders;

public class ProjectEntityBuilder : TestObjectBuilder<ProjectEntityBuilder, ProjectEntity>
{
    private readonly IList<SupportActivityType> _supportActivityTypes = new List<SupportActivityType> { SupportActivityType.AcquiringLand };

    private readonly bool _isFundingRequired = true;

    private readonly RequiredFundingOption _requiredFunding = RequiredFundingOption.Between1MlnAnd5Mln;

    private readonly bool _isProfit = true;

    private readonly AffordableHomesAmount _affordableHomesAmount = AffordableHomesAmount.OnlyAffordableHomes;

    private readonly int _organisationHomesBuilt = 10;

    private readonly IList<InfrastructureType> _infrastructureTypes = new List<InfrastructureType>() { InfrastructureType.Enabling };

    private readonly ProjectGeographicFocus _geographicFocus = ProjectGeographicFocus.Regional;

    private readonly bool _isSiteIdentified = true;

    private readonly bool _isSupportRequired = true;

    private readonly IList<RegionType> _regionTypes = new List<RegionType>() { RegionType.EastMidlands };

    private readonly int _homesNumber = 10;

    private readonly string _expectedStartMonth = "01";

    private readonly string _expectedStartYear = "2022";

    private ProjectEntityBuilder()
        : base(new ProjectEntity(new FrontDoorProjectId("test-id-123"), new ProjectName("Test project")))
    {
    }

    protected override ProjectEntityBuilder Builder => this;

    public static ProjectEntityBuilder New() => new();

    public ProjectEntityBuilder WithSupportActivities(IList<SupportActivityType>? supportActivityTypes = null) =>
        SetProperty(x => x.SupportActivities, new SupportActivities(supportActivityTypes ?? _supportActivityTypes));

    public ProjectEntityBuilder WithRequiredFunding(bool? isFundingRequired = null, RequiredFundingOption? requiredFunding = null)
    {
        _ = SetProperty(x => x.IsFundingRequired, new IsFundingRequired(isFundingRequired ?? _isFundingRequired));
        return SetProperty(x => x.RequiredFunding, new RequiredFunding(requiredFunding ?? _requiredFunding));
    }

    public ProjectEntityBuilder WithIsProfit(bool? isProfit = null) =>
        SetProperty(x => x.IsProfit, new IsProfit(isProfit ?? _isProfit));

    public ProjectEntityBuilder WithAffordableHomesAmount(AffordableHomesAmount? affordableHomesAmount = null) =>
        SetProperty(
            x => x.AffordableHomesAmount,
            new ProjectAffordableHomesAmount(affordableHomesAmount ?? _affordableHomesAmount));

    public ProjectEntityBuilder WithOrganisationHomesBuilt(int? organisationHomesBuilt = null) =>
        SetProperty(
            x => x.OrganisationHomesBuilt,
            new OrganisationHomesBuilt(organisationHomesBuilt ?? _organisationHomesBuilt));

    public ProjectEntityBuilder WithInfrastructureType(IList<InfrastructureType>? infrastructureTypes = null) =>
        SetProperty(
            x => x.Infrastructure,
            new ProjectInfrastructure(infrastructureTypes ?? _infrastructureTypes));

    public ProjectEntityBuilder WithGeographicFocus(ProjectGeographicFocus? geographicFocus = null) =>
        SetProperty(
            x => x.GeographicFocus,
            new Domain.Project.ValueObjects.ProjectGeographicFocus(geographicFocus ?? _geographicFocus));

    public ProjectEntityBuilder WithIsSiteIdentified(bool? isSiteIdentified = null) =>
        SetProperty(
            x => x.IsSiteIdentified,
            new IsSiteIdentified(isSiteIdentified ?? _isSiteIdentified));

    public ProjectEntityBuilder WithIsSupportRequired(bool? isSupportRequired = null) =>
        SetProperty(
            x => x.IsSupportRequired,
            new IsSupportRequired(isSupportRequired ?? _isSupportRequired));

    public ProjectEntityBuilder WithRegions(IList<RegionType>? regionTypes = null) =>
        SetProperty(
            x => x.Regions,
            new Regions(regionTypes ?? _regionTypes));

    public ProjectEntityBuilder WithHomesNumber(int? homesNumber = null) =>
        SetProperty(
            x => x.HomesNumber,
            new HomesNumber(homesNumber ?? _homesNumber));

    public ProjectEntityBuilder WithExpectedStartDate(string? expectedStartMonth = null, string? expectedStartYear = null) =>
        SetProperty(
            x => x.ExpectedStartDate,
            new ExpectedStartDate(expectedStartMonth ?? _expectedStartMonth, expectedStartYear ?? _expectedStartYear));

    public ProjectEntityBuilder WithNonSiteQuestionFulfilled()
    {
        return WithIsSiteIdentified(false)
            .WithGeographicFocus(ProjectGeographicFocus.Regional)
            .WithRegions(new List<RegionType> { RegionType.EastMidlands })
            .WithHomesNumber(35);
    }
}
