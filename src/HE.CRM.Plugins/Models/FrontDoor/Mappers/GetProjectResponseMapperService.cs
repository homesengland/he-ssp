using System;
using System.Collections.Generic;
using System.Linq;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Api.FrontDoor.Contract.Responses;

namespace HE.CRM.Plugins.Models.Frontdoor.Mappers
{
    public class GetProjectResponseMapperService : CrmService, IGetProjectResponseMapperService
    {

        public GetProjectResponseMapperService(CrmServiceArgs args)
            : base(args)
        {
        }

        public FrontDoorProjectDto Map(GetProjectResponse response, Dictionary<Guid, string> contactsExternalIdMap)
        {
            if (response == null)
            {
                return null;
            }

            if (!contactsExternalIdMap.TryGetValue(response.PortalOwnerId, out var portalOwnerExternalId))
            {
                Logger.Warn($"Could not find externalId for contact with id '{response.PortalOwnerId}'");
            }

            return new FrontDoorProjectDto
            {
                ProjectId = response.ProjectRecordId.ToString(),
                ProjectName = response.ProjectName,
                CreatedOn = response.CreatedOn?.UtcDateTime,
                OrganisationId = response.OrganisationId,
                externalId = portalOwnerExternalId,
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
                FrontDoorProjectContact = FrontDoorProjectContactMapper.Map(response.FrontDoorProjectContact, contactsExternalIdMap),
            };
        }
    }
}
