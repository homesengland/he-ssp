namespace HE.Investments.AHP.IntegrationTests.Pages;

public static class ApplicationPagesUrl
{
    public const string TaskListSuffix = "/task-list";

    public const string Start = "ahp/application/start";

    public static string ApplicationName(string siteId) => $"ahp/{siteId}/application/name";

    public static string Tenure(string siteId) => $"ahp/{siteId}/application/tenure";

    public static string TaskList(string applicationId) => $"ahp/application/{applicationId}{TaskListSuffix}";
}
