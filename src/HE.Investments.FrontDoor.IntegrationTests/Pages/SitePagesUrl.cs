namespace HE.Investments.FrontDoor.IntegrationTests.Pages;

internal static class SitePagesUrl
{
    public static string Name(string projectId) => ProjectPagesUrl.BuildProjectPage(projectId, "site");

    public static string HomesNumber(string projectId, string siteId) => BuildSitePage(projectId, siteId, "homes-number");

    public static string LocalAuthority(string projectId, string siteId) => $"apply-for-support/local-authority/search";

    private static string BuildSitePage(string projectId, string siteId, string pageName)
    {
        return ProjectPagesUrl.BuildProjectPage(projectId, $"site/{siteId}/{pageName}");
    }
}
