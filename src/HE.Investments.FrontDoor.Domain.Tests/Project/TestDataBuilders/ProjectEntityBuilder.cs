using HE.Investments.FrontDoor.Domain.Project;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.TestsUtils.TestFramework;
using Microsoft.Crm.Sdk.Messages;
using ProjectGeographicFocus = HE.Investments.FrontDoor.Domain.Project.ValueObjects.ProjectGeographicFocus;
using ProjectInfrastructure = HE.Investments.FrontDoor.Domain.Project.ValueObjects.ProjectInfrastructure;

namespace HE.Investments.FrontDoor.Domain.Tests.Project.TestDataBuilders;

public class ProjectEntityBuilder : TestObjectBuilder<ProjectEntityBuilder, ProjectEntity>
{
    private ProjectEntityBuilder()
        : base(new ProjectEntity(new FrontDoorProjectId("test-id-123"), new ProjectName("Test project")))
    {
    }

    protected override ProjectEntityBuilder Builder => this;

    public static ProjectEntityBuilder New() => new();

    public ProjectEntityBuilder WithSupportActivitiesAsDevelopingHomes()
    {
        Item.ProvideSupportActivityTypes(new SupportActivities(new[] { SupportActivityType.DevelopingHomes }));
        return this;
    }

    public ProjectEntityBuilder WithSupportActivitiesAsProvidingInfrastructure()
    {
        Item.ProvideSupportActivityTypes(new SupportActivities(new[] { SupportActivityType.ProvidingInfrastructure }));
        return this;
    }

    public ProjectEntityBuilder WithRequiredFunding()
    {
        Item.ProvideIsFundingRequired(new IsFundingRequired(true));
        Item.ProvideRequiredFunding(new RequiredFunding(RequiredFundingOption.LessThan250K));
        return this;
    }

    public ProjectEntityBuilder WithIsProfit()
    {
        Item.ProvideIsProfit(new IsProfit(true));
        return this;
    }

    public ProjectEntityBuilder WithAffordableHomesAmount()
    {
        Item.ProvideAffordableHomesAmount(new ProjectAffordableHomesAmount(AffordableHomesAmount.OpenMarkedAndAffordableHomes));
        return this;
    }

    public ProjectEntityBuilder WithInfrastructureTypeUnknown()
    {
        Item.ProvideInfrastructureTypes(new ProjectInfrastructure(new List<InfrastructureType>() { InfrastructureType.IDoNotKnow }));
        return this;
    }

    public ProjectEntityBuilder WithGeographicFocus()
    {
        Item.ProvideGeographicFocus(new ProjectGeographicFocus(Shared.Project.Contract.ProjectGeographicFocus.Regional));
        return this;
    }

    public ProjectEntityBuilder WithIsSiteIdentified(bool isSiteIdentified)
    {
        Item.ProvideIsSiteIdentified(new IsSiteIdentified(isSiteIdentified));
        return this;
    }

    public ProjectEntityBuilder WithRegions()
    {
        Item.ProvideRegions(new Regions(new[] { RegionType.EastMidlands }));
        return this;
    }

    public ProjectEntityBuilder WithHomesNumber()
    {
        Item.ProvideHomesNumber(new HomesNumber(5));
        return this;
    }

    public ProjectEntityBuilder WithNonSiteQuestionFulfilled()
    {
        return WithIsSiteIdentified(false)
            .WithGeographicFocus()
            .WithRegions()
            .WithHomesNumber();
    }
}
