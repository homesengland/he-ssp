namespace HE.Investments.AHP.IntegrationTests.Pages;

public static class ProjectPagesUrl
{
    public const string ProjectList = "ahp/projects";

    public static string ProjectStart(string projectId) => $"ahp/project/start?fdProjectId={projectId}";

    public static string ProjectApplicationList(string projectId) => $"ahp/project/{projectId}/applications";

    public static string ProjectSiteList(string projectId) => $"ahp/project/{projectId}/sites";

    public static string ProjectDetails(string projectId) => $"ahp/project/{projectId}";
}
