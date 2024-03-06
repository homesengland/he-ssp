namespace HE.Investments.FrontDoor.IntegrationTests.Pages;

internal static class SitePagesUrl
{
    public static string HomesNumber(string projectId, string siteId) => BuildSitePage(projectId, siteId, "homes-number");

    private static string BuildSitePage(string projectId, string siteId, string pageName)
    {
        return ProjectPagesUrl.BuildProjectPage(projectId, $"site/{siteId}/{pageName}");
    }
}
