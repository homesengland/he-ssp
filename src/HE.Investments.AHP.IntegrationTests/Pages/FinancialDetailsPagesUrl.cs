namespace HE.Investments.AHP.IntegrationTests.Pages;

public static class FinancialDetailsPagesUrl
{
    public const string Start = "/financial-details/start";

    public const string LandStatusSuffix = "/financial-details/land-status";

    public const string LandValueSuffix = "/financial-details/land-value";

    public const string OtherApplicationCostsSuffix = "/financial-details/other-application-costs";

    public const string ExpectedContributionsSuffix = "/financial-details/expected-contributions";

    public static string LandStatus(string applicationId) => $"ahp/application/{applicationId}{LandStatusSuffix}";

    public static string LandValue(string applicationId) => $"ahp/application/{applicationId}{LandValueSuffix}";

    public static string OtherApplicationCosts(string applicationId) => $"ahp/application/{applicationId}{OtherApplicationCostsSuffix}";

    public static string ExpectedContributions(string applicationId) => $"ahp/application/{applicationId}{ExpectedContributionsSuffix}";
}
