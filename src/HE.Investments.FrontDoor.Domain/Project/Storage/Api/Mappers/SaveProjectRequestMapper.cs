using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Contract;
using HE.Investments.FrontDoor.Domain.Project.Storage.Api.Contract.Requests;

namespace HE.Investments.FrontDoor.Domain.Project.Storage.Api.Mappers;

internal static class SaveProjectRequestMapper
{
    public static SaveProjectRequest Map(FrontDoorProjectDto dto, string organisationId)
    {
        return new SaveProjectRequest
        {
            ProjectRecordId = string.IsNullOrEmpty(dto.ProjectId) ? null : ShortGuid.ToGuid(dto.ProjectId),
            ProjectName = dto.ProjectName,
            OrganisationId = ShortGuid.ToGuid(organisationId),
            PortalOwnerId = Guid.Parse("15e728f9-9af6-46f4-b36e-d36b8ab9fbd7"), // TODO: AB#98936 change to userGlobalId when API will support it
            ProjectSupportsHousingDeliveryInEngland = dto.ProjectSupportsHousingDeliveryinEngland,
            ActivitiesInThisProject = dto.ActivitiesinThisProject?.ToArray(),
            InfrastructureDelivered = dto.InfrastructureDelivered?.ToArray(),
            AmountOfAffordableHomes = dto.AmountofAffordableHomes,
            PreviousResidentialBuildingExperience = dto.PreviousResidentialBuildingExperience,
            IdentifiedSite = dto.IdentifiedSite,
            GeographicFocus = dto.GeographicFocus,
            Region = dto.Region?.ToArray(),
            LocalAuthority = dto.LocalAuthority ?? "6000002", // TODO: AB#98936 remove hardcoded value when API will support it
            NumberOfHomesEnabledBuilt = dto.NumberofHomesEnabledBuilt,
            WouldYourProjectFailWithoutHeSupport = dto.WouldyourprojectfailwithoutHEsupport,
            FundingRequired = dto.FundingRequired,
            AmountOfFundingRequired = dto.AmountofFundingRequired,
            IntentionToMakeAProfit = dto.IntentiontoMakeaProfit,
            StartOfProjectMonth = dto.StartofProjectMonth,
            StartOfProjectYear = dto.StartofProjectYear,
            FrontDoorDecision = dto.FrontDoorDecision != null ? [dto.FrontDoorDecision.Value] : null,
        };
    }
}
