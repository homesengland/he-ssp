using HE.Investments.TestsUtils.Extensions;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Order01StartAhpProjectWithSite.Data;

public class AhpProjectData
{
    public string ProjectId { get; private set; }

    public string ProjectName { get; private set; }

    public string GenerateProjectName(string suffix = "")
    {
        ProjectName = "Project".WithTimestampSuffix() + $" (IT{Suffix(suffix)})";
        return ProjectName;
    }

    public void SetProjectId(string projectId)
    {
        ProjectId = projectId;
    }

    public bool IsProjectNotCreated() => string.IsNullOrEmpty(ProjectId);

    private string Suffix(string suffix)
    {
        if (string.IsNullOrEmpty(suffix))
        {
            return string.Empty;
        }

        return $" {suffix}";
    }
}
