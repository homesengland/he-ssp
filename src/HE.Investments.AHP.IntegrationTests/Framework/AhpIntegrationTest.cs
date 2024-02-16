using HE.Investment.AHP.WWW;
using HE.Investments.AHP.IntegrationTests.FillApplication.Data;
using HE.Investments.AHP.IntegrationTests.FillSite.Data;
using HE.Investments.IntegrationTestsFramework;
using Xunit;

namespace HE.Investments.AHP.IntegrationTests.Framework;

[Collection(nameof(AhpIntegrationTestSharedContext))]
public class AhpIntegrationTest : IntegrationTestBase<Program>
{
    protected AhpIntegrationTest(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        SetApplicationData();
        SetSiteData();
        fixture.CheckUserLoginData();
        fixture.MockUserAccount();
    }

    public ApplicationData ApplicationData { get; private set; }

    public SiteData SiteData { get; private set; }

    private void SetApplicationData()
    {
        var applicationData = GetSharedDataOrNull<ApplicationData>(nameof(ApplicationData));
        if (applicationData is null)
        {
            applicationData = new ApplicationData();
            SetSharedData(nameof(ApplicationData), applicationData);
        }

        ApplicationData = applicationData;
    }

    private void SetSiteData()
    {
        var siteData = GetSharedDataOrNull<SiteData>(nameof(SiteData));
        if (siteData is null)
        {
            siteData = new SiteData();
            SetSharedData(nameof(SiteData), siteData);
        }

        SiteData = siteData;
    }
}
