using HE.Investments.AHP.IntegrationTests.ApplicationManagement.Data.Model;

namespace HE.Investments.AHP.IntegrationTests.ApplicationManagement.Data;

public static class AdminUserInLeadOrganisationOfConsortium
{
    public static UserData UserData { get; } = new(
        "admin.consortium.lead-integration.tests@gmail.com",
        "auth0|665c3f9f607efb43e06f4f6b",
        "2a7ef26a-c320-ef11-840a-000d3a0cff9a",
        "ee269470-c620-ef11-840a-002248c83519");

    public static AhpProject[] Projects => [ProjectAdminLead, ProjectSitePartner];

    public static AhpProject ProjectAdminLead { get; } = new("52cc09f8-c620-ef11-840a-000d3a0cff9a", "AhpProject-Admin-Lead", [SiteLead]);

    public static AhpSite SiteLead => new("de3363fb-c620-ef11-840a-002248c5d15f", "AhpProjectSite-Admin-Lead", [ApplicationLead]);

    public static AhpApplication ApplicationLead => new("AhpApplication-Admin-Lead-01");

    public static AhpProject ProjectSitePartner { get; } = new("9af93cb8-c820-ef11-840a-000d3a0cff9a", "AhpProject-Admin-SitePartner", [SitePartner]);

    public static AhpSite SitePartner => new(
        "8b4a5abe-c820-ef11-840a-002248c5d15f",
        "AhpProjectSite-Admin-SitePartner",
        [ApplicationPartnerForSitePartner, ApplicationSitePartnerForSitePartner]);

    public static AhpApplication ApplicationSitePartnerForSitePartner => new("AhpApplication-Admin-SitePartner-01");

    public static AhpApplication ApplicationPartnerForSitePartner => new("AhpApplication-Admin-ApplicationPartner-02");
}
