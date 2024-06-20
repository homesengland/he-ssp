namespace HE.Investments.AHP.IntegrationTests.Pages;

public static class ApplicationPagesUrl
{
    public const string TaskListSuffix = "/task-list";
    public const string CheckAnswersSuffix = "/check-answers";
    public const string SubmitSuffix = "/submit";
    public const string CompletedSuffix = "/completed";
    public const string RequestToEditSuffix = "/request-to-edit";
    public const string OnHoldSuffix = "/on-hold";
    public const string WithdrawSuffix = "/withdraw";

    public static string Start(string organisationId) => $"ahp/{organisationId}/application/start";

    public static string ApplicationStart(string organisationId, string projectId) => $"ahp/{organisationId}/application/start?projectid={projectId}";

    public static string ApplicationName(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/application/name";

    public static string Tenure(string organisationId, string siteId) => $"ahp/{organisationId}/site/{siteId}/application/tenure";

    public static string TaskList(string organisationId, string applicationId) => $"ahp/{organisationId}/application/{applicationId}{TaskListSuffix}";

    public static string Submit(string organisationId, string applicationId) => $"ahp/{organisationId}/application/{applicationId}{SubmitSuffix}";

    public static string Completed(string organisationId, string applicationId) => $"ahp/{organisationId}/application/{applicationId}{CompletedSuffix}";
}
