namespace HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Pages;

internal static class HomeTypesPagesUrl
{
    public static string LandingPage(string organisationId, string applicationId) => BuildHomeTypesPage(organisationId, applicationId, "home-types");

    public static string List(string organisationId, string applicationId) => BuildHomeTypesPage(organisationId, applicationId, "home-types/list");

    public static string NewHomeType(string organisationId, string applicationId) => BuildHomeTypesPage(organisationId, applicationId, "home-types/details");

    public static string FinishHomeTypes(string organisationId, string applicationId) => BuildHomeTypesPage(organisationId, applicationId, "home-types/finish");

    private static string BuildHomeTypesPage(string organisationId, string applicationId, string pageSuffix)
    {
        return $"ahp/{organisationId}/application/{applicationId}/{pageSuffix}";
    }
}
