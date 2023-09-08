namespace HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;

public static class CompanyStructurePagesUrls
{
    public const string StartCompanyStructureSuffix = "/company/start-company-structure";

    public const string CompanyPurposeSuffix = "/company/purpose";

    public const string MoreInformationAboutOrganizationSuffix = "/company/more-information-about-organization";

    public const string HowManyHomesBuiltSuffix = "/company/how-many-homes-built";

    public static string MoreInformationAboutOrganization(string applicationId) => $"/application/{applicationId}{MoreInformationAboutOrganizationSuffix}";
}
