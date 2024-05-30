using System.Diagnostics;
using HE.Investment.AHP.WWW;
using HE.Investments.AHP.IntegrationTests.Crm;
using HE.Investments.AHP.IntegrationTests.Order01StartAhpProjectWithSite.Data;
using HE.Investments.AHP.IntegrationTests.Order02FillSite.Data;
using HE.Investments.AHP.IntegrationTests.Order03FillApplication.Data;
using HE.Investments.Common.Contract;
using HE.Investments.FrontDoor.IntegrationTests.Utils;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.IntegrationTestsFramework.Auth;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace HE.Investments.AHP.IntegrationTests.Framework;

[Collection(nameof(AhpIntegrationTestSharedContext))]
public class AhpIntegrationTest : IntegrationTestBase<Program>
{
    private readonly ITestOutputHelper _output;

    private readonly AhpIntegrationTestFixture _fixture;

    protected AhpIntegrationTest(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture)
    {
        SetApplicationData();
        SetSiteData();
        SetProjectData();
        InitStopwatch();
        fixture.CheckUserLoginData();
        fixture.MockUserAccount();
        _output = output;
        _fixture = fixture;
        LoginData = fixture.LoginData;
        InFrontDoor = fixture.ServiceProvider.GetRequiredService<FrontDoorDataManipulator>();
    }

    public ApplicationData ApplicationData { get; private set; }

    public SiteData SiteData { get; private set; }

    public AhpProjectData ProjectData { get; private set; }

    public Stopwatch Stopwatch { get; private set; }

    protected AhpCrmContext AhpCrmContext => _fixture.AhpCrmContext;

    protected ILoginData LoginData { get; }

    protected FrontDoorDataManipulator InFrontDoor { get; }

    public override async Task DisposeAsync()
    {
        await base.DisposeAsync();

        _output.WriteLine($"Elapsed time: {Stopwatch.Elapsed.TotalSeconds} sec");
    }

    public async Task ChangeApplicationStatus(string applicationId, ApplicationStatus applicationStatus)
    {
        await AhpCrmContext.ChangeApplicationStatus(applicationId, applicationStatus, LoginData);
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

    private void SetProjectData()
    {
        var projectData = GetSharedDataOrNull<AhpProjectData>(nameof(ProjectData));
        if (projectData is null)
        {
            projectData = new AhpProjectData();
            SetSharedData(nameof(ProjectData), projectData);
        }

        ProjectData = projectData;
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
