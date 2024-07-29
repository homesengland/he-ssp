using HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.ApplicationManagement.Data.Model;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.ApplicationManagement.Data;

public static class AdminUserInLeadOrganisationOfConsortium
{
    public static UserData UserData { get; } = new(
        "admin.consortium.lead-integration.tests@gmail.com",
        "auth0|665c3f9f607efb43e06f4f6b",
        "2a7ef26a-c320-ef11-840a-000d3a0cff9a",
        "ee269470-c620-ef11-840a-002248c83519");

    public static AhpProject[] Projects => AllTestsLimitedConsortium.Projects;

    public static AhpProject ProjectAdminLead => AllTestsLimitedConsortium.ProjectAdminLead;

    public static AhpSite SiteLead => AllTestsLimitedConsortium.SiteLead;

    public static AhpApplication ApplicationLead => AllTestsLimitedConsortium.ApplicationLead;

    public static AhpProject ProjectSitePartner => AllTestsLimitedConsortium.ProjectSitePartner;

    public static AhpSite SitePartner => AllTestsLimitedConsortium.SitePartner;

    public static AhpApplication ApplicationSitePartnerForSitePartner => AllTestsLimitedConsortium.ApplicationSitePartnerForSitePartner;

    public static AhpApplication ApplicationPartnerForSitePartner => AllTestsLimitedConsortium.ApplicationPartnerForSitePartner;
}
