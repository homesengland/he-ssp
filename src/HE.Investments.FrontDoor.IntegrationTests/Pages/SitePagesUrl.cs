namespace HE.Investments.FrontDoor.IntegrationTests.Pages;

internal static class SitePagesUrl
{
    public static string Name(string organisationId, string projectId) => ProjectPagesUrl.BuildProjectPage(organisationId, projectId, "site");

    public static string HomesNumber(string organisationId, string projectId, string siteId) => BuildSitePage(organisationId, projectId, siteId, "homes-number");

    public static string LocalAuthoritySearch(string organisationId) => $"apply-for-support/{organisationId}/local-authority/search";

    public static string LocalAuthorityResult(string organisationId) => $"apply-for-support/{organisationId}/local-authority/search-result";

    public static string LocalAuthorityConfirm(string organisationId, string projectId, string siteId, string localAuthorityCode) => BuildSitePage(organisationId, projectId, siteId, $"local-authority-confirm?localAuthorityCode={localAuthorityCode}");

    public static string LocalAuthorityConfirmSuffix(string organisationId, string projectId, string siteId) => BuildSitePage(organisationId, projectId, siteId, $"local-authority-confirm");

    public static string PlanningStatus(string organisationId, string projectId, string siteId) => BuildSitePage(organisationId, projectId, siteId, "planning-status");

    public static string AddAnotherSite(string organisationId, string projectId, string siteId) => BuildSitePage(organisationId, projectId, siteId, "add-another-site");

    public static string Remove(string organisationId, string projectId, string siteId) => BuildSitePage(organisationId, projectId, siteId, "remove");

    private static string BuildSitePage(string organisationId, string projectId, string siteId, string pageName)
    {
        return ProjectPagesUrl.BuildProjectPage(organisationId, projectId, $"site/{siteId}/{pageName}");
    }
}
