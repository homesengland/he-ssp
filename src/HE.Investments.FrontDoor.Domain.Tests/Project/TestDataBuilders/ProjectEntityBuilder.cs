using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Project.Enums;
using HE.Investments.FrontDoor.Domain.Project;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;
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
}
