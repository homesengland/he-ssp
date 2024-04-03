namespace HE.Investments.FrontDoor.IntegrationTests.Pages;

internal static class SitePagesUrl
{
    public static string Name(string projectId) => ProjectPagesUrl.BuildProjectPage(projectId, "site");

    public static string HomesNumber(string projectId, string siteId) => BuildSitePage(projectId, siteId, "homes-number");

    public static string LocalAuthoritySearch(string projectId, string siteId) => "apply-for-support/local-authority/search";

    public static string LocalAuthorityResult(string projectId, string siteId) => "apply-for-support/local-authority/search-result";

    public static string LocalAuthorityConfirm(string projectId, string siteId, string localAuthorityCode) => BuildSitePage(projectId, siteId, $"local-authority-confirm?localAuthorityCode={localAuthorityCode}");

    public static string LocalAuthorityConfirmSuffix(string projectId, string siteId) => BuildSitePage(projectId, siteId, $"local-authority-confirm");

    public static string PlanningStatus(string projectId, string siteId) => BuildSitePage(projectId, siteId, "planning-status");

    public static string AddAnotherSite(string projectId, string siteId) => BuildSitePage(projectId, siteId, "add-another-site");

    public static string Remove(string projectId, string siteId) => BuildSitePage(projectId, siteId, "remove");

    private static string BuildSitePage(string projectId, string siteId, string pageName)
    {
        return ProjectPagesUrl.BuildProjectPage(projectId, $"site/{siteId}/{pageName}");
    }
}
