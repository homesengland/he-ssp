namespace HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;

public static class ApplicationPagesUrls
{
    public const string StartPage = "/application";

    public const string AboutLoanPage = "/application/about-loan";

    public const string CheckYourDetails = "/application/check-your-details";

    public const string LoanPurpose = "/application/loan-purpose";

    public const string TaskListSuffix = "/task-list";

    public static string TaskList(string applicationId) => $"application/{applicationId}{TaskListSuffix}";
}
