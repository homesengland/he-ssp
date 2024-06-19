using System.Collections.Generic;
using System.Linq;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Api.FrontDoor.Contract.Responses;

namespace HE.CRM.Common.Api.FrontDoor.Mappers
{
    public static class GetProjectResponseMapper
    {
        public static FrontDoorProjectDto Map(GetProjectResponse response)
        {
            // TODO: AB#98936 those 3 properties are not mapped: ProjectType, ProposedInterventions, LeadDirectorate. What should we do with them?
            return new FrontDoorProjectDto
            {
                ProjectId = response.ProjectRecordId.ToString(),
                ProjectName = response.ProjectName,
                CreatedOn = response.CreatedOn?.UtcDateTime,
                OrganisationId = response.OrganisationId,
                externalId = null,
                ProjectSupportsHousingDeliveryinEngland = response.ProjectSupportsHousingDeliveryInEngland,
                ActivitiesinThisProject = response.ActivitiesInThisProject?.ToList() ?? new List<int>(),
                InfrastructureDelivered = response.InfrastructureDelivered?.ToList() ?? new List<int>(),
                AmountofAffordableHomes = response.AmountOfAffordableHomes,
                PreviousResidentialBuildingExperience = response.PreviousResidentialBuildingExperience,
                IdentifiedSite = response.IdentifiedSite,
                GeographicFocus = response.GeographicFocus,
                Region = response.Region?.ToList() ?? new List<int>(),
                LocalAuthority = response.LocalAuthority ?? string.Empty,
                LocalAuthorityName = response.LocalAuthorityName ?? string.Empty,
                LocalAuthorityCode = response.LocalAuthorityCode ?? string.Empty,
                NumberofHomesEnabledBuilt = response.NumberOfHomesEnabledBuilt,
                WouldyourprojectfailwithoutHEsupport = response.WouldYourProjectFailWithoutHeSupport,
                FundingRequired = response.FundingRequired,
                AmountofFundingRequired = response.AmountOfFundingRequired,
                IntentiontoMakeaProfit = response.IntentionToMakeAProfit,
                StartofProjectMonth = response.StartOfProjectMonth,
                StartofProjectYear = response.StartOfProjectYear,
                FrontDoorProjectContact = FrontDoorProjectContactMapper.Map(response.FrontDoorProjectContact),
            };
        }
    }
}
