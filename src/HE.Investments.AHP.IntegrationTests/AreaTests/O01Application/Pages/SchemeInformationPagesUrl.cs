namespace HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Pages;

public static class SchemeInformationPagesUrl
{
    public static string SchemeDetails(string organisationId, string applicationId) => $"ahp/{organisationId}/application/{applicationId}/scheme/start";

    public static string FundingDetails(string organisationId, string applicationId) => $"ahp/{organisationId}/application/{applicationId}/scheme/funding";

    public static string PartnerDetails(string organisationId, string applicationId) =>
        $"ahp/{organisationId}/application/{applicationId}/scheme/partner-details";

    public static string DevelopingPartner(string organisationId, string applicationId) =>
        $"ahp/{organisationId}/application/{applicationId}/scheme/developing-partner";

    public static string DevelopingPartnerConfirmation(string organisationId, string applicationId, string partnerId) =>
        $"ahp/{organisationId}/application/{applicationId}/scheme/developing-partner-confirm/{partnerId}";

    public static string OwnerOfTheLand(string organisationId, string applicationId) =>
        $"ahp/{organisationId}/application/{applicationId}/scheme/owner-of-the-land";

    public static string OwnerOfTheLandConfirmation(string organisationId, string applicationId, string partnerId) =>
        $"ahp/{organisationId}/application/{applicationId}/scheme/owner-of-the-land-confirm/{partnerId}";

    public static string OwnerOfTheHomes(string organisationId, string applicationId) =>
        $"ahp/{organisationId}/application/{applicationId}/scheme/owner-of-the-homes";

    public static string OwnerOfTheHomesConfirmation(string organisationId, string applicationId, string partnerId) =>
        $"ahp/{organisationId}/application/{applicationId}/scheme/owner-of-the-homes-confirm/{partnerId}";

    public static string Affordability(string organisationId, string applicationId) => $"ahp/{organisationId}/application/{applicationId}/scheme/affordability";

    public static string SalesRisk(string organisationId, string applicationId) => $"ahp/{organisationId}/application/{applicationId}/scheme/sales-risk";

    public static string HousingNeeds(string organisationId, string applicationId) => $"ahp/{organisationId}/application/{applicationId}/scheme/housing-needs";

    public static string StakeholderDiscussions(string organisationId, string applicationId) =>
        $"ahp/{organisationId}/application/{applicationId}/scheme/stakeholder-discussions";

    public static string CheckAnswers(string organisationId, string applicationId) => $"ahp/{organisationId}/application/{applicationId}/scheme/check-answers";
}
