namespace HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Pages;

public static class FinancialDetailsPagesUrl
{
    public const string Start = "/financial-details/start";

    public const string LandStatusSuffix = "/financial-details/land-status";

    public const string LandValueSuffix = "/financial-details/land-value";

    public const string OtherApplicationCostsSuffix = "/financial-details/other-application-costs";

    public const string ExpectedContributionsSuffix = "/financial-details/expected-contributions";

    public const string GrantsSuffix = "/financial-details/grants";

    public const string CheckAnswersSuffix = "/financial-details/check-answers";

    public static string LandStatus(string organisationId, string applicationId) => $"ahp/{organisationId}/application/{applicationId}{LandStatusSuffix}";

    public static string LandValue(string organisationId, string applicationId) => $"ahp/{organisationId}/application/{applicationId}{LandValueSuffix}";

    public static string OtherApplicationCosts(string organisationId, string applicationId) =>
        $"ahp/{organisationId}/application/{applicationId}{OtherApplicationCostsSuffix}";

    public static string ExpectedContributions(string organisationId, string applicationId) =>
        $"ahp/{organisationId}/application/{applicationId}{ExpectedContributionsSuffix}";

    public static string Grants(string organisationId, string applicationId) => $"ahp/{organisationId}/application/{applicationId}{GrantsSuffix}";

    public static string CheckAnswers(string organisationId, string applicationId) => $"ahp/{organisationId}/application/{applicationId}{CheckAnswersSuffix}";
}
