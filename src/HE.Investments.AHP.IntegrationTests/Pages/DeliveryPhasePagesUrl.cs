namespace HE.Investments.AHP.IntegrationTests.Pages;

internal static class DeliveryPhasePagesUrl
{
    public static string Name(string organisationId, string applicationId, string deliveryPhaseId) =>
        BuildDeliveryPhasePage(organisationId, applicationId, deliveryPhaseId, "name");

    public static string Details(string organisationId, string applicationId, string deliveryPhaseId) =>
        BuildDeliveryPhasePage(organisationId, applicationId, deliveryPhaseId, "details");

    public static string NewBuildActivityType(string organisationId, string applicationId, string deliveryPhaseId) =>
        BuildDeliveryPhasePage(organisationId, applicationId, deliveryPhaseId, "new-build-activity-type");

    public static string RehabBuildActivityType(string organisationId, string applicationId, string deliveryPhaseId) =>
        BuildDeliveryPhasePage(organisationId, applicationId, deliveryPhaseId, "rehab-build-activity-type");

    public static string ReconfiguringExisting(string organisationId, string applicationId, string deliveryPhaseId) =>
        BuildDeliveryPhasePage(organisationId, applicationId, deliveryPhaseId, "reconfiguring-existing");

    public static string AddHomes(string organisationId, string applicationId, string deliveryPhaseId) =>
        BuildDeliveryPhasePage(organisationId, applicationId, deliveryPhaseId, "add-homes");

    public static string AcquisitionMilestone(string organisationId, string applicationId, string deliveryPhaseId) =>
        BuildDeliveryPhasePage(organisationId, applicationId, deliveryPhaseId, "acquisition-milestone");

    public static string SummaryOfDelivery(string organisationId, string applicationId, string deliveryPhaseId) =>
        BuildDeliveryPhasePage(organisationId, applicationId, deliveryPhaseId, "summary-of-delivery");

    public static string StartOnSiteMilestone(string organisationId, string applicationId, string deliveryPhaseId) =>
        BuildDeliveryPhasePage(organisationId, applicationId, deliveryPhaseId, "start-on-site-milestone");

    public static string PracticalCompletionMilestone(string organisationId, string applicationId, string deliveryPhaseId) =>
        BuildDeliveryPhasePage(organisationId, applicationId, deliveryPhaseId, "practical-completion-milestone");

    public static string UnregisteredBodyFollowUp(string organisationId, string applicationId, string deliveryPhaseId) =>
        BuildDeliveryPhasePage(organisationId, applicationId, deliveryPhaseId, "unregistered-body-follow-up");

    public static string Remove(string organisationId, string applicationId, string deliveryPhaseId) =>
        BuildDeliveryPhasePage(organisationId, applicationId, deliveryPhaseId, "remove");

    public static string CheckAnswers(string organisationId, string applicationId, string deliveryPhaseId) =>
        BuildDeliveryPhasePage(organisationId, applicationId, deliveryPhaseId, "check-answers");

    private static string BuildDeliveryPhasePage(string organisationId, string applicationId, string deliveryPhaseId, string pageSuffix)
    {
        return $"ahp/{organisationId}/application/{applicationId}/delivery-phase/{deliveryPhaseId}/{pageSuffix}";
    }
}
