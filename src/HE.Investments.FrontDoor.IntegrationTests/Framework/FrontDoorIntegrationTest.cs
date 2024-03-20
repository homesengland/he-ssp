using HE.Investments.FrontDoor.IntegrationTests.FillProject.Data;
using HE.Investments.FrontDoor.WWW;
using HE.Investments.IntegrationTestsFramework;
using Xunit;

namespace HE.Investments.FrontDoor.IntegrationTests.Framework;

[Collection(nameof(FrontDoorIntegrationTestSharedContext))]
public class FrontDoorIntegrationTest : IntegrationTestBase<Program>
{
    protected FrontDoorIntegrationTest(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        SetProjectData();
        fixture.CheckUserLoginData();
        fixture.MockUserAccount();
    }

    public ProjectData ProjectData { get; private set; }

    public SiteData SiteData => ProjectData.SiteData;

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
}
