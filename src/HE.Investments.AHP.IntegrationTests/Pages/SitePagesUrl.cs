namespace HE.Investments.AHP.IntegrationTests.Pages;

public static class SitePagesUrl
{
    public const string SiteStart = $"/ahp/site/start";

    public const string SiteName = $"/ahp/site/name";

    public static string SiteSection106GeneralAgreement(string siteId) => $"ahp/site/{siteId}/section-106-general-agreement";

    public static string SiteSection106AffordableHousing(string siteId) => $"ahp/site/{siteId}/section-106-affordable-housing";

    public static string SiteSection106OnlyAffordableHousing(string siteId) => $"ahp/site/{siteId}/section-106-only-affordable-housing";

    public static string SiteSection106AdditionalAffordableHousing(string siteId) => $"ahp/site/{siteId}/section-106-additional-affordable-housing";

    public static string SiteSection106CapitalFundingEligibility(string siteId) => $"ahp/site/{siteId}/section-106-capital-funding-eligibility";

    public static string SiteSection106LocalAuthorityConfirmation(string siteId) => $"ahp/site/{siteId}/section-106-local-authority-confirmation";
}
