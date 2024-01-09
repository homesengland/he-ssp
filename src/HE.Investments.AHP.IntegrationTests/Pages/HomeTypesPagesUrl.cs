namespace HE.Investments.AHP.IntegrationTests.Pages;

internal static class HomeTypesPagesUrl
{
    public static string LandingPage(string applicationId) => BuildHomeTypesPage(applicationId, "HomeTypes");

    public static string List(string applicationId) => BuildHomeTypesPage(applicationId, "HomeTypes/List");

    public static string NewHomeType(string applicationId) => BuildHomeTypesPage(applicationId, "HomeTypes/HomeTypeDetails");

    public static string FinishHomeTypes(string applicationId) => BuildHomeTypesPage(applicationId, "HomeTypes/FinishHomeTypes");

    private static string BuildHomeTypesPage(string applicationId, string pageSuffix)
    {
        return $"ahp/application/{applicationId}/{pageSuffix}";
    }
}
