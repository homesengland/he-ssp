extern alias Org;

using System.Collections.ObjectModel;
using System.Globalization;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.FrontDoor.Shared.Project.Crm;
using HE.Investments.FrontDoor.Shared.Project.Data;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.FrontDoor.Shared.Project.Repositories;

internal class PrefillDataRepository : IPrefillDataRepository
{
    private readonly IProjectCrmContext _crmContext;

    private readonly IFrontDoorProjectEnumMapping _mapping;

    public PrefillDataRepository(IProjectCrmContext crmContext, IFrontDoorProjectEnumMapping mapping)
    {
        _crmContext = crmContext;
        _mapping = mapping;
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

        var site = project.IdentifiedSite == true
            ? await _crmContext.GetProjectSite(projectId.Value, cancellationToken)
            : null;

        return Map(project, site);
    }

    public async Task MarkProjectAsUsed(FrontDoorProjectId projectId, CancellationToken cancellationToken)
    {
        await _crmContext.DeactivateProject(projectId.Value, cancellationToken);
    }

    private static SiteDetails MapSiteDetails(FrontDoorProjectSiteDto site)
    {
        return new SiteDetails(site.SiteName);
    }

    private static DateDetails MapDateDetails(int? month, int? year)
    {
        return new DateDetails("01", month?.ToString(CultureInfo.InvariantCulture) ?? "01", year?.ToString(CultureInfo.InvariantCulture) ?? "1900");
    }

    private ProjectPrefillData Map(FrontDoorProjectDto project, FrontDoorProjectSiteDto? site)
    {
        var isSiteIdentified = project.IdentifiedSite ?? false;
        var isFundingRequired = project.FundingRequired ?? false;

        return new ProjectPrefillData(
            new FrontDoorProjectId(project.ProjectId),
            project.ProjectSupportsHousingDeliveryinEngland ?? true,
            project.ProjectName,
            new ReadOnlyCollection<SupportActivityType>(DomainEnumMapper.Map(project.ActivitiesinThisProject, _mapping.ActivityType)),
            DomainEnumMapper.Map(project.AmountofAffordableHomes, _mapping.AffordableHomes),
            project.NumberofHomesEnabledBuilt,
            new ReadOnlyCollection<InfrastructureType>(DomainEnumMapper.Map(project.InfrastructureDelivered, _mapping.Infrastructure)),
            isSiteIdentified,
            isSiteIdentified && site != null ? MapSiteDetails(site) : null,
            isSiteIdentified ? null : MapSiteNotIdentifiedDetails(project),
            project.WouldyourprojectfailwithoutHEsupport ?? false,
            isFundingRequired,
            isFundingRequired ? MapFundingDetails(project) : null,
            MapDateDetails(project.StartofProjectMonth, project.StartofProjectYear));
    }

    private SiteNotIdentifiedDetails MapSiteNotIdentifiedDetails(FrontDoorProjectDto project)
    {
        return new SiteNotIdentifiedDetails(
            DomainEnumMapper.Map(project.GeographicFocus, _mapping.GeographicFocus) ?? ProjectGeographicFocus.Undefined,
            new ReadOnlyCollection<RegionType>(DomainEnumMapper.Map(project.Region, _mapping.RegionType)),
            project.NumberofHomesEnabledBuilt ?? 0);
    }

    private FundingDetails MapFundingDetails(FrontDoorProjectDto project)
    {
        return new FundingDetails(
            DomainEnumMapper.Map(project.AmountofFundingRequired, _mapping.FundingAmount) ?? RequiredFundingOption.Undefined,
            project.IntentiontoMakeaProfit ?? false);
    }
}
