namespace HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Pages;

internal static class DeliveryPhasesPagesUrl
{
    public static string LandingPage(string organisationId, string applicationId) => BuildDeliveryPhasesPage(organisationId, applicationId, "delivery/start");

    public static string List(string organisationId, string applicationId) => BuildDeliveryPhasesPage(organisationId, applicationId, "delivery");

    public static string NewDeliveryPhase(string organisationId, string applicationId) =>
        BuildDeliveryPhasesPage(organisationId, applicationId, "delivery-phase/create");

    public static string CompleteDeliveryPhases(string organisationId, string applicationId) =>
        BuildDeliveryPhasesPage(organisationId, applicationId, "delivery/complete");

    private static string BuildDeliveryPhasesPage(string organisationId, string applicationId, string pageSuffix)
    {
        return $"ahp/{organisationId}/application/{applicationId}/{pageSuffix}";
    }
}
