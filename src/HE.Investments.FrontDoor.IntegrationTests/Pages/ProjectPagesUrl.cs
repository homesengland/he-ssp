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

    public static string GeographicFocus(string projectId) => BuildProjectPage(projectId, "geographic-focus");

    public static string Region(string projectId) => BuildProjectPage(projectId, "region");

    public static string HomesNumber(string projectId) => BuildProjectPage(projectId, "homes-number");

    public static string Progress(string projectId) => BuildProjectPage(projectId, "progress");

    public static string RequiresFunding(string projectId) => BuildProjectPage(projectId, "requires-funding");

    public static string FundingAmount(string projectId) => BuildProjectPage(projectId, "funding-amount");

    public static string Profit(string projectId) => BuildProjectPage(projectId, "profit");

    public static string ExpectedStart(string projectId) => BuildProjectPage(projectId, "expected-start");

    public static string CheckAnswers(string projectId) => BuildProjectPage(projectId, "check-answers");

    public static string YouNeedToSpeakToHomesEngland(string projectId) => BuildProjectPage(projectId, "you-need-to-speak-to-homes-england");

    public static string BuildProjectPage(string projectId, string pageName)
    {
        return $"apply-for-support/project/{projectId}/{pageName}";
    }
}
