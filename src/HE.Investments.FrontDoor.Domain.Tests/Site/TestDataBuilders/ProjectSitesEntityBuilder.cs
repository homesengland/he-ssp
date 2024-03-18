using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Site;
using HE.Investments.FrontDoor.Domain.Site;
using HE.Investments.FrontDoor.Domain.Site.ValueObjects;
using HE.Investments.FrontDoor.Domain.Tests.Project.TestDataBuilders;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investments.FrontDoor.Domain.Tests.Site.TestDataBuilders;

public class ProjectSitesEntityBuilder : TestObjectBuilder<ProjectSitesEntityBuilder, ProjectSitesEntity>
{
    public ProjectSitesEntityBuilder(FrontDoorProjectId projectId)
        : base(new ProjectSitesEntity(projectId))
    {
    }

    protected override ProjectSitesEntityBuilder Builder => this;

    public static ProjectSitesEntityBuilder New(FrontDoorProjectId? projectId = null) => new(projectId ?? FrontDoorProjectIdTestData.IdOne);

    public ProjectSitesEntityBuilder WithSite(SiteName siteName, FrontDoorSiteId? siteId = null)
    {
        Item.Sites.Add(ProjectSiteEntityBuilder.New(siteName, Item.ProjectId, siteId).Build());
        return this;
    }
}
