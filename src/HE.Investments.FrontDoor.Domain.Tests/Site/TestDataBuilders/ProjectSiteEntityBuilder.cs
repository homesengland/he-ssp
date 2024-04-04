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
    private readonly string _siteName = "Test site";

    private readonly int _homesNumber = 10;

    private readonly SitePlanningStatus _planningStatus = SitePlanningStatus.DetailedPlanningApprovalGranted;

    private readonly string _localAuthorityCode = "E08000012";

    private readonly string _localAuthorityName = "Liverpool";

    public ProjectSiteEntityBuilder(FrontDoorProjectId projectId, FrontDoorSiteId siteId, SiteName siteName)
        : base(new ProjectSiteEntity(projectId, siteId, siteName))
    {
    }

    protected override ProjectSiteEntityBuilder Builder => this;

    public static ProjectSiteEntityBuilder New(SiteName? siteName, FrontDoorProjectId? projectId, FrontDoorSiteId? siteId) =>
        new(projectId ?? FrontDoorProjectIdTestData.IdOne, siteId ?? FrontDoorSiteIdTestData.IdOne, siteName ?? new SiteName("Test site"));

    public ProjectSiteEntityBuilder WithSiteName(string? siteName = null) =>
        SetProperty(x => x.Name, new SiteName(siteName ?? _siteName));

    public ProjectSiteEntityBuilder WithHomesNumber(int? homesNumber = null) =>
        SetProperty(
            x => x.HomesNumber,
            new HomesNumber(homesNumber ?? _homesNumber));

    public ProjectSiteEntityBuilder WithPlanningStatus(SitePlanningStatus? planningStatus = null) =>
        SetProperty(
            x => x.PlanningStatus,
            new PlanningStatus(planningStatus ?? _planningStatus));

    public ProjectSiteEntityBuilder WithLocalAuthority(string? localAuthorityCode = null, string? localAuthorityName = null) => SetProperty(
        x => x.LocalAuthority,
        new SiteLocalAuthority(new LocalAuthorityCode(localAuthorityCode ?? _localAuthorityCode), localAuthorityName ?? _localAuthorityName));
}
