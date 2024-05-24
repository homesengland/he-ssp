namespace HE.Investments.AHP.IntegrationTests.Pages;

public static class ApplicationPagesUrl
{
    public const string Start = "ahp/application/start";
    public const string TaskListSuffix = "/task-list";
    public const string CheckAnswersSuffix = "/check-answers";
    public const string SubmitSuffix = "/submit";
    public const string CompletedSuffix = "/completed";
    public const string RequestToEditSuffix = "/request-to-edit";
    public const string OnHoldSuffix = "/on-hold";
    public const string WithdrawSuffix = "/withdraw";

    public static string ApplicationStart(string projectId) => $"ahp/application/start?projectid={projectId}";

    public static string ApplicationName(string siteId) => $"ahp/{siteId}/application/name";

    public static string Tenure(string siteId) => $"ahp/{siteId}/application/tenure";

    public static string TaskList(string applicationId) => $"ahp/application/{applicationId}{TaskListSuffix}";

    public static string Submit(string applicationId) => $"ahp/application/{applicationId}{SubmitSuffix}";

    public static string Completed(string applicationId) => $"ahp/application/{applicationId}{CompletedSuffix}";
}
