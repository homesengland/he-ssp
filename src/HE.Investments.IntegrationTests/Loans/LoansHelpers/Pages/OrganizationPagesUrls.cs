namespace HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;

public static class OrganizationPagesUrls
{
    public const string CompleteProfileDetails = "user/profile-details";

    public const string OrganizationSearch = "organization/search";

    public const string SearchOrganizationResult = "organization/search/result";

    public static string ConfirmOrganization(string organizationNumber) => $"organization/{organizationNumber}/confirm";
}
