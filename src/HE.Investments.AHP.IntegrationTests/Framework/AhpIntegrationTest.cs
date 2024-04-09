using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW;
using HE.Investments.AHP.IntegrationTests.Crm;
using HE.Investments.AHP.IntegrationTests.FillApplication.Data;
using HE.Investments.AHP.IntegrationTests.FillSite.Data;
using HE.Investments.Common.Contract;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.IntegrationTestsFramework.Auth;
using Xunit;
using Xunit.Abstractions;

namespace HE.Investments.AHP.IntegrationTests.Framework;

[SuppressMessage("Usage", "CA1816", Justification = "It is not needed here")]
[SuppressMessage("Design", "CA1063", Justification = "It is not needed here")]
[SuppressMessage("Code Smell", "S3881", Justification = "It is not needed here")]
[Collection(nameof(AhpIntegrationTestSharedContext))]
public class AhpIntegrationTest : IntegrationTestBase<Program>, IDisposable
{
    private readonly ITestOutputHelper _output;

    private readonly AhpIntegrationTestFixture _fixture;

    protected AhpIntegrationTest(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture)
    {
        SetApplicationData();
        SetSiteData();
        InitStopwatch();
        fixture.CheckUserLoginData();
        fixture.MockUserAccount();
        _output = output;
        _fixture = fixture;
        LoginData = fixture.LoginData;
    }

    public ApplicationData ApplicationData { get; private set; }

    public SiteData SiteData { get; private set; }

    public Stopwatch Stopwatch { get; private set; }

    protected AhpApplicationCrmContext AhpApplicationCrmContext => _fixture.AhpApplicationCrmContext;

    private ILoginData LoginData { get; }

    public void Dispose()
    {
        _output.WriteLine($"Elapsed time: {Stopwatch.Elapsed.TotalSeconds} sec");
    }

    public async Task ChangeApplicationStatus(string applicationId, ApplicationStatus applicationStatus)
    {
        await AhpApplicationCrmContext.ChangeApplicationStatus(applicationId, applicationStatus, LoginData);
    }

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

    private void InitStopwatch()
    {
        var stopwatch = GetSharedDataOrNull<Stopwatch>(nameof(Stopwatch));
        if (stopwatch is null)
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
            SetSharedData(nameof(Stopwatch), stopwatch);
        }

        Stopwatch = stopwatch;
    }
}
