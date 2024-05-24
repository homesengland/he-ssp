namespace HE.Investments.AHP.IntegrationTests.Pages;

public static class ProjectPagesUrl
{
    public static string ProjectStart(string projectId) => $"ahp/project/start?fdProjectId={projectId}";

    public static string ProjectApplicationList(string projectId) => $"ahp/project/{projectId}/applications";
}
