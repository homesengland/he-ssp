extern alias Org;

using HE.Investments.Common.Contract.Enum;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Domain.Site;
using HE.Investments.FrontDoor.Domain.Site.ValueObjects;
using HE.Investments.FrontDoor.Domain.Tests.Project.TestDataBuilders;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.TestsUtils.TestFramework;
using Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects;
using SiteLocalAuthority = Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects.LocalAuthority;

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

    public ProjectSiteEntityBuilder WithSiteName(string siteName) => SetProperty(x => x.Name, new SiteName(siteName));

    public ProjectSiteEntityBuilder WithHomesNumber(int homesNumber) => SetProperty(x => x.HomesNumber, new HomesNumber(homesNumber));

    public ProjectSiteEntityBuilder WithPlanningStatus(SitePlanningStatus planningStatus) => SetProperty(x => x.PlanningStatus, new PlanningStatus(planningStatus));

    public ProjectSiteEntityBuilder WithLocalAuthority(string localAuthorityCode, string localAuthorityName) => SetProperty(x => x.LocalAuthority, new SiteLocalAuthority(new LocalAuthorityId(localAuthorityCode), localAuthorityName));
}
