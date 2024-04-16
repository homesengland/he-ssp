extern alias Org;

using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Shared.Project;
using ProjectLocalAuthority = Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects.LocalAuthority;

namespace HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;

public class ProjectCrmMapper : IProjectCrmMapper
{
    private readonly SupportActivitiesMapper _supportActivitiesMapper = new();

    private readonly AffordableHomesAmountMapper _affordableHomesAmountMapper = new();

    private readonly ProjectInfrastructureMapper _projectInfrastructureMapper = new();

    private readonly RegionsMapper _regionsMapper = new();

    private readonly ProjectGeographicFocusMapper _projectGeographicFocusMapper = new();

    private readonly RequiredFundingMapper _requiredFundingMapper = new();

    public FrontDoorProjectDto ToDto(ProjectEntity entity, UserAccount userAccount)
    {
        return new FrontDoorProjectDto
        {
            ProjectId = entity.Id.IsNew ? null : entity.Id.Value,
            ProjectName = entity.Name.Value,
            ProjectSupportsHousingDeliveryinEngland = entity.IsEnglandHousingDelivery,
            OrganisationId = userAccount.SelectedOrganisationId().Value,
            externalId = userAccount.UserGlobalId.Value,
            ActivitiesinThisProject = _supportActivitiesMapper.Map(entity.SupportActivities),
            AmountofAffordableHomes = _affordableHomesAmountMapper.ToDto(entity.AffordableHomesAmount.AffordableHomesAmount),
            InfrastructureDelivered = _projectInfrastructureMapper.Map(entity.Infrastructure),
            PreviousResidentialBuildingExperience = entity.OrganisationHomesBuilt?.Value,
            IdentifiedSite = entity.IsSiteIdentified?.Value,
            Region = _regionsMapper.Map(entity.Regions),
            NumberofHomesEnabledBuilt = entity.HomesNumber?.Value,
            LocalAuthorityCode = entity.LocalAuthority?.Code.Value,
            LocalAuthorityName = entity.LocalAuthority?.Name,
            GeographicFocus = _projectGeographicFocusMapper.ToDto(entity.GeographicFocus.GeographicFocus),
            WouldyourprojectfailwithoutHEsupport = entity.IsSupportRequired?.Value,
            FundingRequired = entity.IsFundingRequired?.Value,
            AmountofFundingRequired = _requiredFundingMapper.Map(entity.RequiredFunding),
            IntentiontoMakeaProfit = entity.IsProfit.Value,
            StartofProjectMonth = entity.ExpectedStartDate.Value?.Month,
            StartofProjectYear = entity.ExpectedStartDate.Value?.Year,
        };
    }

    public ProjectEntity ToEntity(FrontDoorProjectDto dto)
    {
        return new ProjectEntity(
            new FrontDoorProjectId(dto.ProjectId),
            new ProjectName(dto.ProjectName),
            dto.ProjectSupportsHousingDeliveryinEngland,
            supportActivityTypes: _supportActivitiesMapper.Map(dto.ActivitiesinThisProject),
            infrastructureTypes: _projectInfrastructureMapper.Map(dto.InfrastructureDelivered),
            affordableHomesAmount: ProjectAffordableHomesAmount.Create(_affordableHomesAmountMapper.ToDomain(dto.AmountofAffordableHomes)),
            organisationHomesBuilt: dto.PreviousResidentialBuildingExperience.IsProvided() ? new OrganisationHomesBuilt((int)dto.PreviousResidentialBuildingExperience!) : null,
            isSiteIdentified: dto.IdentifiedSite.IsProvided() ? new IsSiteIdentified(dto.IdentifiedSite) : null,
            regions: _regionsMapper.Map(dto.Region),
            homesNumber: dto.NumberofHomesEnabledBuilt.IsProvided() ? new HomesNumber(dto.NumberofHomesEnabledBuilt!.Value) : null,
            geographicFocus: ProjectGeographicFocus.Create(_projectGeographicFocusMapper.ToDomain(dto.GeographicFocus)),
            isSupportRequired: dto.WouldyourprojectfailwithoutHEsupport.IsProvided() ? new IsSupportRequired(dto.WouldyourprojectfailwithoutHEsupport) : null,
            isFundingRequired: dto.FundingRequired.IsProvided() ? new IsFundingRequired(dto.FundingRequired) : null,
            requiredFunding: _requiredFundingMapper.Map(dto.AmountofFundingRequired),
            isProfit: dto.IntentiontoMakeaProfit.IsProvided() ? new IsProfit(dto.IntentiontoMakeaProfit) : null,
            expectedStartDate: ExpectedStartDate.Create(dto.StartofProjectMonth, dto.StartofProjectYear),
            localAuthority: string.IsNullOrWhiteSpace(dto.LocalAuthorityCode) ? null : ProjectLocalAuthority.New(dto.LocalAuthorityCode, dto.LocalAuthorityName));
    }
}
