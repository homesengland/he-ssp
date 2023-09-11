namespace HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;

public static class CompanyStructurePagesUrls
{
    public const string StartCompanyStructureSuffix = "/company/start-company-structure";

    public const string CompanyPurposeSuffix = "/company/purpose";

    public const string MoreInformationAboutOrganizationSuffix = "/company/more-information-about-organization";

    public const string HowManyHomesBuiltSuffix = "/company/how-many-homes-built";

    public const string CheckYourAnswersSuffix = "/company/check-answers";

    public static string CompanyPurpose(string applicationId) => $"/application/{applicationId}{CompanyPurposeSuffix}";

    public static string MoreInformationAboutOrganization(string applicationId) => $"/application/{applicationId}{MoreInformationAboutOrganizationSuffix}";

    public static string HowManyHomesBuilt(string applicationId) => $"/application/{applicationId}{HowManyHomesBuiltSuffix}";

    public static string CheckYourAnswers(string applicationId) => $"/application/{applicationId}{CheckYourAnswersSuffix}";

}
