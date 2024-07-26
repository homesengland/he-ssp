namespace HE.Investments.AHP.IntegrationTests.Pages;

public static class ClaimsPagesUrl
{
    public static string Summary(string organisationId, string allocationId) =>
        $"ahp/{organisationId}/allocation/{allocationId}/claims/summary";

    public static string Overview(string organisationId, string allocationId, string phaseId) =>
        $"ahp/{organisationId}/allocation/{allocationId}/claims/{phaseId}/overview";
}
