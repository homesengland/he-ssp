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

    public PrefillDataRepository(IProjectCrmContext crmContext)
    {
        _crmContext = crmContext;
    }

    public async Task<ProjectPrefillData> GetProjectPrefillData(
        FrontDoorProjectId projectId,
        UserAccount userAccount,
        CancellationToken cancellationToken)
    {
        var organisationId = userAccount.SelectedOrganisationId().Value;
        var project = userAccount.CanViewAllApplications()
            ? await _crmContext.GetOrganisationProjectById(projectId.Value, userAccount.UserGlobalId.Value, organisationId, cancellationToken)
            : await _crmContext.GetUserProjectById(projectId.Value, userAccount.UserGlobalId.Value, organisationId, cancellationToken);

        return Map(project);
    }

    private static ProjectPrefillData Map(FrontDoorProjectDto project)
    {
        var isSiteIdentified = project.IdentifiedSite ?? false;
        var isFundingRequired = project.FundingRequired ?? false;

        return new ProjectPrefillData(
            new FrontDoorProjectId(project.ProjectId),
            project.ProjectSupportsHousingDeliveryinEngland ?? true,
            project.ProjectName,
            new ReadOnlyCollection<SupportActivityType>(DomainEnumMapper.Map(project.ActivitiesinThisProject, FrontDoorProjectEnumMapping.ActivityType)),
            DomainEnumMapper.Map(project.AmountofAffordableHomes, FrontDoorProjectEnumMapping.AffordableHomes),
            project.NumberofHomesEnabledBuilt,
            new ReadOnlyCollection<InfrastructureType>(DomainEnumMapper.Map(project.InfrastructureDelivered, FrontDoorProjectEnumMapping.Infrastructure)),
            isSiteIdentified,
            isSiteIdentified ? null : MapSiteNotIdentifiedDetails(project),
            project.WouldyourprojectfailwithoutHEsupport ?? false,
            isFundingRequired,
            isFundingRequired ? MapFundingDetails(project) : null,
            MapDateDetails(project.StartofProjectMonth, project.StartofProjectYear));
    }

    private static SiteNotIdentifiedDetails MapSiteNotIdentifiedDetails(FrontDoorProjectDto project)
    {
        return new SiteNotIdentifiedDetails(
            DomainEnumMapper.Map(project.GeographicFocus, FrontDoorProjectEnumMapping.GeographicFocus) ?? ProjectGeographicFocus.Undefined,
            new ReadOnlyCollection<RegionType>(DomainEnumMapper.Map(project.Region, FrontDoorProjectEnumMapping.RegionType)),
            project.NumberofHomesEnabledBuilt ?? 0);
    }

    private static FundingDetails MapFundingDetails(FrontDoorProjectDto project)
    {
        return new FundingDetails(
            DomainEnumMapper.Map(project.AmountofFundingRequired, FrontDoorProjectEnumMapping.FundingAmount) ?? RequiredFundingOption.Undefined,
            project.IntentiontoMakeaProfit ?? false);
    }

    private static DateDetails MapDateDetails(int? month, int? year)
    {
        return new DateDetails("01", month?.ToString(CultureInfo.InvariantCulture) ?? "01", year?.ToString(CultureInfo.InvariantCulture) ?? "1900");
    }
}
