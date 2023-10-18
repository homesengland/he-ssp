namespace HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;

public static class ApplicationPagesUrls
{
    public const string StartPage = "/application";

    public const string AboutLoanPage = "/application/about-loan";

    public const string CheckYourDetails = "/application/check-your-details";

    public const string LoanPurpose = "/application/loan-purpose";

    public const string ApplicationName = "/application/application-name";

    public const string TaskListSuffix = "/task-list";

    public const string CheckApplicationSuffix = "/check";

    public const string ApplicationSubmittedSuffix = "/submitted";

    public const string WithdrawSuffix = "/withdraw";

    public static string TaskList(string applicationId) => $"application/{applicationId}{TaskListSuffix}";

    public static string ApplicationDashboard(string applicationId) => $"application/{applicationId}/dashboard";
}
