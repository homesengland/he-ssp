using System;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Api.FrontDoor.Contract.Requests;

namespace HE.CRM.Common.Api.FrontDoor.Mappers
{
    public static class SaveProjectRequestMapper
    {
        public static SaveProjectRequest Map(FrontDoorProjectDto dto, Guid userId)
        {
            return new SaveProjectRequest
            {
                ProjectRecordId = string.IsNullOrEmpty(dto.ProjectId) ? (Guid?)null : Guid.Parse(dto.ProjectId),
                ProjectName = dto.ProjectName,
                OrganisationId = dto.OrganisationId,
                PortalOwnerId = userId,
                ProjectSupportsHousingDeliveryInEngland = dto.ProjectSupportsHousingDeliveryinEngland,
                ActivitiesInThisProject = dto.ActivitiesinThisProject?.ToArray(),
                InfrastructureDelivered = dto.InfrastructureDelivered?.ToArray(),
                AmountOfAffordableHomes = dto.AmountofAffordableHomes,
                PreviousResidentialBuildingExperience = dto.PreviousResidentialBuildingExperience,
                IdentifiedSite = dto.IdentifiedSite,
                GeographicFocus = dto.GeographicFocus,
                Region = dto.Region?.ToArray(),
                LocalAuthority = dto.LocalAuthorityCode,
                NumberOfHomesEnabledBuilt = dto.NumberofHomesEnabledBuilt,
                WouldYourProjectFailWithoutHeSupport = dto.WouldyourprojectfailwithoutHEsupport,
                FundingRequired = dto.FundingRequired,
                AmountOfFundingRequired = dto.AmountofFundingRequired,
                IntentionToMakeAProfit = dto.IntentiontoMakeaProfit,
                StartOfProjectMonth = dto.StartofProjectMonth,
                StartOfProjectYear = dto.StartofProjectYear,
                FrontDoorDecision = dto.EligibleApplication?.ToArray()
            };
        }
    }
}
