using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using HE.Investments.FrontDoor.IntegrationTests.FillProject.Data;
using HE.Investments.FrontDoor.WWW;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.IntegrationTestsFramework.Auth;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace HE.Investments.FrontDoor.IntegrationTests.Framework;

[SuppressMessage("Usage", "CA1816", Justification = "It is not needed here")]
[SuppressMessage("Design", "CA1063", Justification = "It is not needed here")]
[SuppressMessage("Code Smell", "S3881", Justification = "It is not needed here")]
[Collection(nameof(FrontDoorIntegrationTestSharedContext))]
public class FrontDoorIntegrationTest : IntegrationTestBase<Program>, IDisposable
{
    protected FrontDoorIntegrationTest(FrontDoorIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture)
    {
        Output = output;
        SetProjectData();
        InitStopwatch();
        fixture.CheckUserLoginData();
        fixture.MockUserAccount();
        LoginData = fixture.LoginData;
        InCrm = fixture.ServiceProvider.GetRequiredService<DataManipulator>();
    }

    public ProjectData ProjectData { get; private set; }

    public SiteData SiteData => ProjectData.SiteData;

    public SiteData SecondSiteData => ProjectData.SecondSiteData;

    public Stopwatch Stopwatch { get; private set; }

    protected ILoginData LoginData { get; }

    protected ITestOutputHelper Output { get; }

    protected DataManipulator InCrm { get; }

    public void Dispose()
    {
        Output.WriteLine($"Elapsed time: {Stopwatch.Elapsed.TotalSeconds} sec");
    }

    private void SetProjectData()
    {
        var projectData = GetSharedDataOrNull<ProjectData>(nameof(ProjectData));
        if (projectData is null)
        {
            projectData = new ProjectData();
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
