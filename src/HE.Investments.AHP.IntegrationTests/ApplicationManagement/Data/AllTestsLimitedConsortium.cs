using HE.Investments.AHP.IntegrationTests.ApplicationManagement.Data.Model;

namespace HE.Investments.AHP.IntegrationTests.ApplicationManagement.Data;

public static class AllTestsLimitedConsortium
{
    public static AhpProject[] Projects => [ProjectAdminLead, ProjectSitePartner];

    public static AhpProject ProjectAdminLead { get; } = new("52cc09f8-c620-ef11-840a-000d3a0cff9a", "AhpProject-Admin-Lead", [SiteLead]);

    public static AhpSite SiteLead => new("de3363fb-c620-ef11-840a-002248c5d15f", "AhpProjectSite-Admin-Lead", [ApplicationLead]);

    public static AhpApplication ApplicationLead => new("c8cd8015-c720-ef11-840a-002248c5d15f", "AhpApplication-Admin-Lead-01");

    public static AhpProject ProjectSitePartner { get; } = new("9af93cb8-c820-ef11-840a-000d3a0cff9a", "AhpProject-Admin-SitePartner", [SitePartner]);

    public static AhpSite SitePartner => new(
        "8b4a5abe-c820-ef11-840a-002248c5d15f",
        "AhpProjectSite-Admin-SitePartner",
        [ApplicationPartnerForSitePartner, ApplicationSitePartnerForSitePartner]);

    public static AhpApplication ApplicationSitePartnerForSitePartner => new("792badd6-c820-ef11-840a-000d3a0cff9a", "AhpApplication-Admin-SitePartner-01");

    public static AhpApplication ApplicationPartnerForSitePartner => new("b1a0f0e8-c920-ef11-840a-000d3a0cff9a", "AhpApplication-Admin-ApplicationPartner-02");
}
