extern alias Org;

using System.Collections.ObjectModel;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.FrontDoor.Shared.Project.Crm;
using HE.Investments.FrontDoor.Shared.Project.Data;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.FrontDoor.Shared.Project.Repositories;

internal class PrefillDataRepository : IPrefillDataRepository
{
    private readonly IProjectCrmContext _crmContext;

    private readonly IFrontDoorProjectEnumMapping _mapping;

    private readonly IPlanningStatusMapper _planningStatusMapper;

    public PrefillDataRepository(IProjectCrmContext crmContext, IFrontDoorProjectEnumMapping mapping, IPlanningStatusMapper planningStatusMapper)
    {
        _crmContext = crmContext;
        _mapping = mapping;
        _planningStatusMapper = planningStatusMapper;
    }

    public async Task<ProjectPrefillData> GetProjectPrefillData(
        FrontDoorProjectId projectId,
        UserAccount userAccount,
        CancellationToken cancellationToken)
    {
        var organisationId = userAccount.SelectedOrganisationId().Value;
        var project = userAccount.CanViewAllApplications()
            ? await _crmContext.GetOrganisationProjectById(projectId.Value, organisationId, cancellationToken)
            : await _crmContext.GetUserProjectById(projectId.Value, userAccount.UserGlobalId.Value, organisationId, cancellationToken);

        var sites = project.IdentifiedSite == true
            ? await _crmContext.GetProjectSites(projectId.Value, cancellationToken)
            : null;

        return Map(project, sites?.items.FirstOrDefault());
    }

    public async Task<SitePrefillData> GetSitePrefillData(FrontDoorProjectId projectId, FrontDoorSiteId siteId, CancellationToken cancellationToken)
    {
        var site = await _crmContext.GetProjectSite(projectId.Value, siteId.Value, cancellationToken);

        return new SitePrefillData(
            siteId,
            site.SiteName,
            site.NumberofHomesEnabledBuilt,
            _planningStatusMapper.ToDomain(site.PlanningStatus) ?? SitePlanningStatus.Undefined,
            site.LocalAuthorityName);
    }

    public async Task MarkProjectAsUsed(FrontDoorProjectId projectId, CancellationToken cancellationToken)
    {
        await _crmContext.DeactivateProject(projectId.Value, cancellationToken);
    }

    private ProjectPrefillData Map(FrontDoorProjectDto project, FrontDoorProjectSiteDto? site)
    {
        return new ProjectPrefillData(
            new FrontDoorProjectId(project.ProjectId),
            project.ProjectName,
            new ReadOnlyCollection<SupportActivityType>(DomainEnumMapper.Map(project.ActivitiesinThisProject, _mapping.ActivityType)),
            site.IsProvided() ? new FrontDoorSiteId(site!.SiteId) : null);
    }
}
