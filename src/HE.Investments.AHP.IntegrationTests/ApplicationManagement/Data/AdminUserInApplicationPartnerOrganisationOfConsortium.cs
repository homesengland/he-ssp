using HE.Investments.AHP.IntegrationTests.ApplicationManagement.Data.Model;

namespace HE.Investments.AHP.IntegrationTests.ApplicationManagement.Data;

public static class AdminUserInApplicationPartnerOrganisationOfConsortium
{
    public static UserData UserData { get; } = new(
        "admin.consortium.application.partner-app01-integration.tests@gmail.com",
        "auth0|665c43a9e627bbe9ed3b2fcf",
        "03da109a-c920-ef11-840a-000d3a0cff9a",
        "ee269470-c620-ef11-840a-002248c83519");

    public static AhpProject[] Projects => [AllTestsLimitedConsortium.ProjectSitePartner];

    public static AhpProject ProjectSitePartner => AllTestsLimitedConsortium.ProjectSitePartner with { Sites = [SitePartner] };

    public static AhpSite SitePartner => AllTestsLimitedConsortium.SitePartner with { Applications = [ApplicationPartnerForSitePartner] };

    public static AhpApplication ApplicationPartnerForSitePartner => AllTestsLimitedConsortium.ApplicationPartnerForSitePartner;
}
