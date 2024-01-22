namespace HE.Investments.AHP.IntegrationTests.Pages;

public static class SitePagesUrl
{
    public const string SiteSelect = "ahp/site/select";

    public static string SiteConfirm(string siteId) => $"ahp/site/{siteId}/confirm-select";
}
