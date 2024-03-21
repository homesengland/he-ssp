using HE.Investments.FrontDoor.Domain.Site;
using HE.Investments.FrontDoor.Domain.Site.ValueObjects;
using HE.Investments.FrontDoor.Domain.Tests.Project.TestDataBuilders;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investments.FrontDoor.Domain.Tests.Site.TestDataBuilders;

public class ProjectSiteEntityBuilder : TestObjectBuilder<ProjectSiteEntityBuilder, ProjectSiteEntity>
{
    public ProjectSiteEntityBuilder(FrontDoorProjectId projectId, FrontDoorSiteId siteId, SiteName siteName)
        : base(new ProjectSiteEntity(projectId, siteId, siteName))
    {
    }

    protected override ProjectSiteEntityBuilder Builder => this;

    public static ProjectSiteEntityBuilder New(SiteName? siteName, FrontDoorProjectId? projectId, FrontDoorSiteId? siteId) =>
        new(projectId ?? FrontDoorProjectIdTestData.IdOne, siteId ?? FrontDoorSiteIdTestData.IdOne, siteName ?? new SiteName("Test site"));
}
