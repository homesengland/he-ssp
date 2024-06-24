namespace HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;

public static class PagesUrls
{
    public const string MainPage = "/";

    public static string DashboardPage(string organisationId) => $"/{organisationId}/dashboard";
}
