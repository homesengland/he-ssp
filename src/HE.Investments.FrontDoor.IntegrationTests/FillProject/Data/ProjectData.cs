using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.TestsUtils.Extensions;

namespace HE.Investments.FrontDoor.IntegrationTests.FillProject.Data;

public class ProjectData
{
    public string Id { get; private set; }

    public string Name { get; private set; }

    public SupportActivityType ActivityType => SupportActivityType.DevelopingHomes;

    public void SetProjectId(string projectId)
    {
        Id = projectId;
    }

    public string GenerateProjectName()
    {
        Name = "IT-Project".WithTimestampSuffix();
        return Name;
    }
}
