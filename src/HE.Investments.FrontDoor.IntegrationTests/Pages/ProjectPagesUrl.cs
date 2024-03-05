namespace HE.Investments.FrontDoor.IntegrationTests.Pages;

internal static class ProjectPagesUrl
{
    public static string Start => "apply-for-support/project/start";

    public static string NewEnglandHousingDelivery => "apply-for-support/project/new-england-housing-delivery";

    public static string NewName => "apply-for-support/project/new-name";

    public static string SupportRequiredActivities(string projectId) => BuildProjectPage(projectId, "support-required-activities");

    public static string Tenure(string projectId) => BuildProjectPage(projectId, "tenure");

    public static string OrganisationHomesBuilt(string projectId) => BuildProjectPage(projectId, "organisation-homes-built");

    public static string IdentifiedSite(string projectId) => BuildProjectPage(projectId, "identified-site");

    public static string BuildProjectPage(string projectId, string pageName)
    {
        return $"apply-for-support/project/{projectId}/{pageName}";
    }
}
