namespace HE.Investments.AHP.IntegrationTests.Pages;

public static class SitePagesUrl
{
    public const string SiteStart = "/ahp/site/start";

    public const string SiteName = "/ahp/site/name";

    public const string SiteSelect = "ahp/site/select";

    public static string SiteSection106GeneralAgreement(string siteId) => $"ahp/site/{siteId}/section-106-general-agreement";

    public static string SiteSection106AffordableHousing(string siteId) => $"ahp/site/{siteId}/section-106-affordable-housing";

    public static string SiteSection106OnlyAffordableHousing(string siteId) => $"ahp/site/{siteId}/section-106-only-affordable-housing";

    public static string SiteSection106AdditionalAffordableHousing(string siteId) => $"ahp/site/{siteId}/section-106-additional-affordable-housing";

    public static string SiteSection106CapitalFundingEligibility(string siteId) => $"ahp/site/{siteId}/section-106-capital-funding-eligibility";

    public static string SiteSection106LocalAuthorityConfirmation(string siteId) => $"ahp/site/{siteId}/section-106-local-authority-confirmation";

    public static string SiteLocalAuthoritySearch(string siteId) => $"ahp/site/{siteId}/local-authority/search";

    public static string SiteLocalAuthorityResult(string siteId) => $"ahp/site/{siteId}/local-authority/search/result";

    public static string SiteLocalAuthorityConfirm(string siteId, string localAuthorityId, string localAuthorityName, string phrase) =>
        $"ahp/site/{siteId}/local-authority/{localAuthorityId}/{localAuthorityName}/confirm?phrase={phrase}";

    public static string SitePlanningStatus(string siteId) => $"ahp/site/{siteId}/planning-status";

    public static string SiteConfirm(string siteId) => $"ahp/site/{siteId}/confirm-select";

    public static string SiteNationaDesignGuide(string siteId) => $"ahp/site/{siteId}/national-design-guide";
}
