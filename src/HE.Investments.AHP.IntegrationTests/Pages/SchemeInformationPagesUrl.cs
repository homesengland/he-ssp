namespace HE.Investments.AHP.IntegrationTests.Pages;

public static class SchemeInformationPagesUrl
{
    public static string SchemeDetails(string applicationId) => $"ahp/application/{applicationId}/scheme/start";

    public static string FundingDetails(string applicationId) => $"ahp/application/{applicationId}/scheme/funding";

    public static string PartnerDetails(string applicationId) => $"ahp/application/{applicationId}/scheme/partner-details";

    public static string DevelopingPartner(string applicationId) => $"ahp/application/{applicationId}/scheme/developing-partner";

    public static string DevelopingPartnerConfirmation(string applicationId, string organisationId) => $"ahp/application/{applicationId}/scheme/developing-partner-confirm/{organisationId}";

    public static string OwnerOfTheLand(string applicationId) => $"ahp/application/{applicationId}/scheme/owner-of-the-land";

    public static string OwnerOfTheLandConfirmation(string applicationId, string organisationId) => $"ahp/application/{applicationId}/scheme/owner-of-the-land-confirm/{organisationId}";

    public static string OwnerOfTheHomes(string applicationId) => $"ahp/application/{applicationId}/scheme/owner-of-the-homes";

    public static string OwnerOfTheHomesConfirmation(string applicationId, string organisationId) => $"ahp/application/{applicationId}/scheme/owner-of-the-homes-confirm/{organisationId}";

    public static string Affordability(string applicationId) => $"ahp/application/{applicationId}/scheme/affordability";

    public static string SalesRisk(string applicationId) => $"ahp/application/{applicationId}/scheme/sales-risk";

    public static string HousingNeeds(string applicationId) => $"ahp/application/{applicationId}/scheme/housing-needs";

    public static string StakeholderDiscussions(string applicationId) => $"ahp/application/{applicationId}/scheme/stakeholder-discussions";

    public static string CheckAnswers(string applicationId) => $"ahp/application/{applicationId}/scheme/check-answers";
}
