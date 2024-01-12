namespace HE.Investments.AHP.IntegrationTests.Pages;

public static class ApplicationPagesUrl
{
    public const string TaskListSuffix = "/task-list";

    public const string StartName = "ahp/application/start";

    public const string ApplicationName = "ahp/application/name";

    public const string Tenure = "ahp/application/tenure";

    public static string TaskList(string applicationId) => $"ahp/application/{applicationId}{TaskListSuffix}";
}
