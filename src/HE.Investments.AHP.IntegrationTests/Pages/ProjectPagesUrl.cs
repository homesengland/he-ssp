namespace HE.Investments.AHP.IntegrationTests.Pages;

public static class ProjectPagesUrl
{
    public static string ProjectList(string organisationId) => $"ahp/{organisationId}/projects";

    public static string ProjectStart(string organisationId, string projectId) => $"ahp/{organisationId}/project/start?fdProjectId={projectId}";

    public static string ProjectApplicationList(string organisationId, string projectId) => $"ahp/{organisationId}/project/{projectId}/applications";

    public static string ProjectSiteList(string organisationId, string projectId) => $"ahp/{organisationId}/project/{projectId}/sites";

    public static string ProjectDetails(string organisationId, string projectId) => $"ahp/{organisationId}/project/{projectId}";
}
