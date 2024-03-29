namespace HE.Investments.AHP.IntegrationTests.Pages;

internal static class DeliveryPhasePagesUrl
{
    public static string Name(string applicationId, string deliveryPhaseId) => BuildDeliveryPhasePage(applicationId, deliveryPhaseId, "name");

    public static string Details(string applicationId, string deliveryPhaseId) => BuildDeliveryPhasePage(applicationId, deliveryPhaseId, "details");

    public static string RehabBuildActivityType(string applicationId, string deliveryPhaseId) =>
        BuildDeliveryPhasePage(applicationId, deliveryPhaseId, "rehab-build-activity-type");

    public static string ReconfiguringExisting(string applicationId, string deliveryPhaseId) =>
        BuildDeliveryPhasePage(applicationId, deliveryPhaseId, "reconfiguring-existing");

    public static string AddHomes(string applicationId, string deliveryPhaseId) =>
        BuildDeliveryPhasePage(applicationId, deliveryPhaseId, "add-homes");

    public static string AcquisitionMilestone(string applicationId, string deliveryPhaseId) =>
        BuildDeliveryPhasePage(applicationId, deliveryPhaseId, "acquisition-milestone");

    public static string SummaryOfDelivery(string applicationId, string deliveryPhaseId) =>
        BuildDeliveryPhasePage(applicationId, deliveryPhaseId, "summary-of-delivery");

    public static string StartOnSiteMilestone(string applicationId, string deliveryPhaseId) =>
        BuildDeliveryPhasePage(applicationId, deliveryPhaseId, "start-on-site-milestone");

    public static string PracticalCompletionMilestone(string applicationId, string deliveryPhaseId) =>
        BuildDeliveryPhasePage(applicationId, deliveryPhaseId, "practical-completion-milestone");

    public static string UnregisteredBodyFollowUp(string applicationId, string deliveryPhaseId) =>
        BuildDeliveryPhasePage(applicationId, deliveryPhaseId, "unregistered-body-follow-up");

    public static string Remove(string applicationId, string deliveryPhaseId) => BuildDeliveryPhasePage(applicationId, deliveryPhaseId, "remove");

    public static string CheckAnswers(string applicationId, string deliveryPhaseId) => BuildDeliveryPhasePage(applicationId, deliveryPhaseId, "check-answers");

    private static string BuildDeliveryPhasePage(string applicationId, string deliveryPhaseId, string pageSuffix)
    {
        return $"ahp/application/{applicationId}/delivery-phase/{deliveryPhaseId}/{pageSuffix}";
    }
}
