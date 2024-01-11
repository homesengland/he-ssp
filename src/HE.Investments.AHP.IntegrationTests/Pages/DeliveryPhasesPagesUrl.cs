namespace HE.Investments.AHP.IntegrationTests.Pages;

internal static class DeliveryPhasesPagesUrl
{
    public static string LandingPage(string applicationId) => BuildDeliveryPhasesPage(applicationId, "delivery/start");

    public static string List(string applicationId) => BuildDeliveryPhasesPage(applicationId, "delivery");

    public static string NewDeliveryPhase(string applicationId) => BuildDeliveryPhasesPage(applicationId, "delivery-phase/create");

    public static string CompleteDeliveryPhases(string applicationId) => BuildDeliveryPhasesPage(applicationId, "delivery/complete");

    private static string BuildDeliveryPhasesPage(string applicationId, string pageSuffix)
    {
        return $"ahp/application/{applicationId}/{pageSuffix}";
    }
}
