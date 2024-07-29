using HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.ApplicationManagement.Data.Model;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.ApplicationManagement.Data;

public static class AllTestsLimitedConsortium
{
    public static AhpProject[] Projects => [ProjectAdminLead, ProjectSitePartner];

    public static AhpProject ProjectAdminLead { get; } = new("52cc09f8-c620-ef11-840a-000d3a0cff9a", "AhpProject-Admin-Lead", [SiteLead]);

    public static AhpSite SiteLead => new("de3363fb-c620-ef11-840a-002248c5d15f", "AhpProjectSite-Admin-Lead", [ApplicationLead]);

    public static AhpApplication ApplicationLead => new("1e03553c-1033-ef11-8e4e-6045bd0b0565", "AhpApplication-Admin-Lead-01");

    public static AhpProject ProjectSitePartner { get; } = new("9af93cb8-c820-ef11-840a-000d3a0cff9a", "AhpProject-Admin-SitePartner", [SitePartner]);

    public static AhpSite SitePartner => new(
        "8b4a5abe-c820-ef11-840a-002248c5d15f",
        "AhpProjectSite-Admin-SitePartner",
        [ApplicationPartnerForSitePartner, ApplicationSitePartnerForSitePartner]);

    public static AhpApplication ApplicationSitePartnerForSitePartner => new("792badd6-c820-ef11-840a-000d3a0cff9a", "AhpApplication-Admin-SitePartner-01");

    public static AhpApplication ApplicationPartnerForSitePartner => new("e3b94518-1233-ef11-8e4e-6045bd0b0565", "AhpApplication-Admin-ApplicationPartner-02");
}
