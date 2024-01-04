namespace HE.Investments.AHP.IntegrationTests.Pages;

public static class SchemaInformationPagesUrl
{
    public const string SchemaDetails = "/scheme/start";

    public const string FundingDetailsSuffix = "/scheme/funding";

    public const string AffordabilitySuffix = "/scheme/affordability";

    public const string SalesRiskSuffix = "/scheme/sales-risk";

    public const string HousingNeedsSuffix = "/scheme/housing-needs";

    public const string StakeholderDiscussionsSuffix = "/scheme/stakeholder-discussions";

    public const string CheckAnswersSuffix = "/scheme/check-answers";

    public static string FundingDetails(string applicationId) => $"ahp/application/{applicationId}{FundingDetailsSuffix}";

    public static string Affordability(string applicationId) => $"ahp/application/{applicationId}{AffordabilitySuffix}";

    public static string SalesRisk(string applicationId) => $"ahp/application/{applicationId}{SalesRiskSuffix}";

    public static string HousingNeeds(string applicationId) => $"ahp/application/{applicationId}{HousingNeedsSuffix}";

    public static string StakeholderDiscussions(string applicationId) => $"ahp/application/{applicationId}{StakeholderDiscussionsSuffix}";

    public static string CheckAnswers(string applicationId) => $"ahp/application/{applicationId}{CheckAnswersSuffix}";
}
