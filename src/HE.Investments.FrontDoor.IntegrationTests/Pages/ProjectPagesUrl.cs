namespace HE.Investments.FrontDoor.IntegrationTests.Pages;

internal static class ProjectPagesUrl
{
    public static string Start(string organisationId) => $"apply-for-support/{organisationId}/project/start";

    public static string NewEnglandHousingDelivery(string organisationId) => $"apply-for-support/{organisationId}/project/new-england-housing-delivery";

    public static string NewName(string organisationId) => $"apply-for-support/{organisationId}/project/new-name";

    public static string SupportRequiredActivities(string organisationId, string projectId) => BuildProjectPage(organisationId, projectId, "support-required-activities");

    public static string Tenure(string organisationId, string projectId) => BuildProjectPage(organisationId, projectId, "tenure");

    public static string OrganisationHomesBuilt(string organisationId, string projectId) => BuildProjectPage(organisationId, projectId, "organisation-homes-built");

    public static string IdentifiedSite(string organisationId, string projectId) => BuildProjectPage(organisationId, projectId, "identified-site");

    public static string GeographicFocus(string organisationId, string projectId) => BuildProjectPage(organisationId, projectId, "geographic-focus");

    public static string Region(string organisationId, string projectId) => BuildProjectPage(organisationId, projectId, "region");

    public static string HomesNumber(string organisationId, string projectId) => BuildProjectPage(organisationId, projectId, "homes-number");

    public static string Progress(string organisationId, string projectId) => BuildProjectPage(organisationId, projectId, "progress");

    public static string RequiresFunding(string organisationId, string projectId) => BuildProjectPage(organisationId, projectId, "requires-funding");

    public static string FundingAmount(string organisationId, string projectId) => BuildProjectPage(organisationId, projectId, "funding-amount");

    public static string Profit(string organisationId, string projectId) => BuildProjectPage(organisationId, projectId, "profit");

    public static string ExpectedStart(string organisationId, string projectId) => BuildProjectPage(organisationId, projectId, "expected-start");

    public static string CheckAnswers(string organisationId, string projectId) => BuildProjectPage(organisationId, projectId, "check-answers");

    public static string YouNeedToSpeakToHomesEngland(string organisationId, string projectId) => BuildProjectPage(organisationId, projectId, "you-need-to-speak-to-homes-england");

    public static string BuildProjectPage(string organisationId, string projectId, string pageName)
    {
        return $"apply-for-support/{organisationId}/project/{projectId}/{pageName}";
    }
}
