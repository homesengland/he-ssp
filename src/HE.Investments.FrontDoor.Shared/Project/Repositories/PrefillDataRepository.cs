using System.Collections.ObjectModel;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.FrontDoor.Shared.Project.Data;
using HE.Investments.FrontDoor.Shared.Project.Storage.Crm;

namespace HE.Investments.FrontDoor.Shared.Project.Repositories;

internal sealed class PrefillDataRepository : IPrefillDataRepository
{
    private readonly IProjectContext _context;

    public PrefillDataRepository(IProjectContext context)
    {
        _context = context;
    }

    public async Task<ProjectPrefillData> GetProjectPrefillData(
        FrontDoorProjectId projectId,
        UserAccount userAccount,
        CancellationToken cancellationToken)
    {
        var organisationId = userAccount.SelectedOrganisationId().Value;
        var project = userAccount.CanViewAllApplications()
            ? await _context.GetOrganisationProjectById(projectId.Value, organisationId, cancellationToken, true)
            : await _context.GetUserProjectById(projectId.Value, userAccount.UserGlobalId.Value, organisationId, cancellationToken, true);

        var sites = project.IdentifiedSite == true
            ? await _context.GetProjectSites(projectId.Value, cancellationToken)
            : null;

        return Map(project, sites?.items);
    }

    public async Task<SitePrefillData> GetSitePrefillData(FrontDoorProjectId projectId, FrontDoorSiteId siteId, CancellationToken cancellationToken)
    {
        var site = await _context.GetProjectSite(projectId.Value, siteId.Value, cancellationToken);

        return new SitePrefillData(
            siteId,
            site.SiteName,
            site.NumberofHomesEnabledBuilt,
            DomainEnumMapper.Map(site.PlanningStatus, FrontDoorProjectEnumMapping.PlanningStatus, SitePlanningStatus.Undefined)!.Value,
            site.LocalAuthorityName);
    }

    public async Task MarkProjectAsUsed(FrontDoorProjectId projectId, CancellationToken cancellationToken)
    {
        await _context.DeactivateProject(projectId.Value, cancellationToken);
    }

    private ProjectPrefillData Map(FrontDoorProjectDto project, IList<FrontDoorProjectSiteDto>? frontDoorSites)
    {
        var sites = frontDoorSites?
            .Select(x =>
                new SitePrefillData(
                    FrontDoorSiteId.From(x.SiteId),
                    x.SiteName,
                    x.NumberofHomesEnabledBuilt,
                    DomainEnumMapper.Map(x.PlanningStatus, FrontDoorProjectEnumMapping.PlanningStatus, SitePlanningStatus.Undefined)!.Value,
                    x.LocalAuthorityName))
            .ToList() ?? [];

        return new ProjectPrefillData(
            FrontDoorProjectId.From(project.ProjectId),
            project.ProjectName,
            new ReadOnlyCollection<SupportActivityType>(DomainEnumMapper.Map(project.ActivitiesinThisProject, FrontDoorProjectEnumMapping.ActivityType)),
            sites);
    }
}
