namespace HE.Investments.AHP.IntegrationTests.Pages;

internal static class HomeTypesPagesUrl
{
    public static string LandingPage(string applicationId) => BuildHomeTypesPage(applicationId, "home-types");

    public static string List(string applicationId) => BuildHomeTypesPage(applicationId, "home-types/list");

    public static string NewHomeType(string applicationId) => BuildHomeTypesPage(applicationId, "home-types/details");

    public static string FinishHomeTypes(string applicationId) => BuildHomeTypesPage(applicationId, "home-types/finish");

    private static string BuildHomeTypesPage(string applicationId, string pageSuffix)
    {
        return $"ahp/application/{applicationId}/{pageSuffix}";
    }
}
