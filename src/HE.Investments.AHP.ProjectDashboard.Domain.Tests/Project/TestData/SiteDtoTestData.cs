using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Tests.Project.TestData;

public static class SiteDtoTestData
{
    public static readonly SiteDto FirstSite = new() { id = "00000000-0000-1111-1111-111111111111", name = "First site", };

    public static readonly SiteDto SecondSite = new() { id = "00000000-0000-2222-2222-111111111112", name = "Second site", };

    public static readonly SiteDto ThirdSite = new() { id = "00000000-0000-3333-3333-111111111113", name = "Third site", };

    public static readonly SiteDto FourthSite = new() { id = "00000000-0000-4444-4444-111111111114", name = "Fourth site", };
}
