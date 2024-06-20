namespace HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;

internal static class SecurityPageUrls
{
    public const string StartSuffix = "/security";

    public const string ChargesDebtSuffix = "/security/charges-debt";

    public const string DirectorLoansSuffix = "/security/dir-loans";

    public const string DirLoansSubSuffix = "/security/dir-loans-sub";

    public const string CheckYourAnswersSuffix = "/security/check-answers";

    public static string ChargesDebt(string organisationId, string applicationId) => $"/{organisationId}/application/{applicationId}{ChargesDebtSuffix}";

    public static string DirectorLoans(string organisationId, string applicationId) => $"/{organisationId}/application/{applicationId}{DirectorLoansSuffix}";

    public static string DirLoansSub(string organisationId, string applicationId) => $"/{organisationId}/application/{applicationId}{DirLoansSubSuffix}";

    public static string CheckYourAnswers(string organisationId, string applicationId) =>
        $"/{organisationId}/application/{applicationId}{CheckYourAnswersSuffix}";
}
