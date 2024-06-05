using HE.Investments.AHP.IntegrationTests.ApplicationManagement.Data.Model;

namespace HE.Investments.AHP.IntegrationTests.ApplicationManagement.Data;

public static class AdminUserInSitePartnerOrganisationOfConsortium
{
    public static UserData UserData { get; } = new(
        "admin.consortium.site.partner-app01-integration.tests@gmail.com",
        "auth0|665c43a9e627bbe9ed3b2fcf",
        "e28ea7ba-c720-ef11-840a-002248c5d15f",
        "ee269470-c620-ef11-840a-002248c83519");

    public static AhpProject[] Projects => [AllTestsLimitedConsortium.ProjectSitePartner];

    public static AhpProject ProjectSitePartner => AllTestsLimitedConsortium.ProjectSitePartner;

    public static AhpSite SitePartner => AllTestsLimitedConsortium.SitePartner;

    public static AhpApplication ApplicationSitePartnerForSitePartner => AllTestsLimitedConsortium.ApplicationSitePartnerForSitePartner;

    public static AhpApplication ApplicationPartnerForSitePartner => AllTestsLimitedConsortium.ApplicationPartnerForSitePartner;
}
