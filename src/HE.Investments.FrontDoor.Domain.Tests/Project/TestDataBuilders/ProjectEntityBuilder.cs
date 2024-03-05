using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Domain.Project;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investments.FrontDoor.Domain.Tests.Project.TestDataBuilders;

public class ProjectEntityBuilder : TestObjectBuilder<ProjectEntityBuilder, ProjectEntity>
{
    private ProjectEntityBuilder()
        : base(new ProjectEntity(new FrontDoorProjectId("test-id-123"), new ProjectName("Test project")))
    {
    }

    protected override ProjectEntityBuilder Builder => this;

    public static ProjectEntityBuilder New() => new();
}
