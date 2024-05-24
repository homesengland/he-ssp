using HE.Investments.TestsUtils.Extensions;

namespace HE.Investments.AHP.IntegrationTests.StartAhpProjectWithSite.Data;

public class AhpProjectData
{
    public string ProjectId { get; private set; }

    public string ProjectName { get; private set; }

    public string GenerateProjectName()
    {
        ProjectName = $"Project".WithTimestampSuffix() + "(IT)";
        return ProjectName;
    }

    public void SetProjectId(string projectId)
    {
        ProjectId = projectId;
    }
}
