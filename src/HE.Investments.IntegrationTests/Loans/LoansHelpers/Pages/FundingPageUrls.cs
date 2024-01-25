namespace HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
internal static class FundingPageUrls
{
    public const string StartFundingSuffix = "/funding/start-funding";

    public const string GrossDevelopmentValueSuffix = "/funding/gross-development-value";

    public const string EstimatedTotalCostsSuffix = "/funding/estimated-total-costs";

    public const string AbnormalCostsSuffix = "/funding/abnormal-costs";

    public const string PrivateSectorFundingSuffix = "/funding/funding-private-sector";

    public const string RepaymentSystemSuffix = "/funding/repayment-system";

    public const string AdditionalProjectsSuffix = "/funding/additional-projects";

    public const string CheckYourAnswersSuffix = "/funding/check-answers";

    public static string StartFunding(string applicationId) => $"/application/{applicationId}{StartFundingSuffix}";

    public static string GrossDevelopmentValue(string applicationId) => $"/application/{applicationId}{GrossDevelopmentValueSuffix}";

    public static string EstimatedTotalCosts(string applicationId) => $"/application/{applicationId}{EstimatedTotalCostsSuffix}";

    public static string AbnormalCosts(string applicationId) => $"/application/{applicationId}{AbnormalCostsSuffix}";

    public static string PrivateSectorFunding(string applicationId) => $"/application/{applicationId}{PrivateSectorFundingSuffix}";

    public static string RepaymentSystem(string applicationId) => $"/application/{applicationId}{RepaymentSystemSuffix}";

    public static string AdditionalProjects(string applicationId) => $"/application/{applicationId}{AdditionalProjectsSuffix}";

    public static string CheckYourAnswers(string applicationId) => $"/application/{applicationId}{CheckYourAnswersSuffix}";
}
