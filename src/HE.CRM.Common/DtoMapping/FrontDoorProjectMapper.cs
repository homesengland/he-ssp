using System;
using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Extensions.Entities;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Common.DtoMapping
{
    public class FrontDoorProjectMapper
    {

        public static FrontDoorProjectDto MapRegularEntityToDto(invln_FrontDoorProjectPOC project, List<FrontDoorProjectSiteDto> frontDoorProjectSiteDtoList, string externalContactId, Contact contact = null)
        {
            var frontDoorProjectDto = new FrontDoorProjectDto()
            {
                ProjectId = project.Id.ToString(),
                externalId = externalContactId,
                OrganisationId = project.invln_AccountId != null ? project.invln_AccountId.Id : Guid.Empty,
                ProjectName = project.invln_Name,
                ProjectSupportsHousingDeliveryinEngland = project.invln_ProjectSupportsHousingDeliveryinEngland,
                ActivitiesinThisProject = new List<int>(),
                InfrastructureDelivered = new List<int>(),
                AmountofAffordableHomes = project.invln_AmountofAffordableHomes?.Value,
                PreviousResidentialBuildingExperience = project.invln_PreviousResidentialBuildingExperience,
                IdentifiedSite = project.invln_IdentifiedSite,
                GeographicFocus = project.invln_GeographicFocus?.Value,
                Region = new List<int>(),
                LocalAuthority = Guid.Empty,
                NumberofHomesEnabled_Built = project.invln_NumberofHomesEnabledBuilt,
                WouldyourprojectfailwithoutHEsupport = project.invln_WouldyourprojectfailwithoutHEsupport,
                FundingRequired = project.invln_FundingRequired,
                AmountofFundingRequired = project.invln_AmountofFundingRequired?.Value,
                StartofProjectMonth = project.invln_StartofProjectMonth,
                StartofProjectYear = project.invln_StartofProjectYear,
                FrontDoorSiteList = frontDoorProjectSiteDtoList,
            };

            if (project.invln_ActivitiesinThisProject != null && project.invln_ActivitiesinThisProject.Any())
            {
                foreach (var activitiinThisProjec in project.invln_ActivitiesinThisProject)
                {
                    frontDoorProjectDto.ActivitiesinThisProject.Add(activitiinThisProjec.Value);
                }
            }
            if (project.invln_InfrastructureDelivered != null && project.invln_InfrastructureDelivered.Any())
            {
                foreach (var infrastructureDelivered in project.invln_InfrastructureDelivered)
                {
                    frontDoorProjectDto.InfrastructureDelivered.Add(infrastructureDelivered.Value);
                }
            }
            if (project.invln_Region != null && project.invln_Region.Any())
            {
                foreach (var region in project.invln_Region)
                {
                    frontDoorProjectDto.Region.Add(region.Value);
                }
            }

            if (contact != null)
            {
                frontDoorProjectDto.FrontDoorProjectContact = new UserAccountDto()
                {
                    ContactEmail = contact.EMailAddress1,
                    ContactFirstName = contact.FirstName,
                    ContactLastName = contact.LastName,
                    ContactExternalId = contact.invln_externalid,
                    ContactTelephoneNumber = contact.Telephone1,
                };
            }

            return frontDoorProjectDto;
        }

        public static invln_FrontDoorProjectPOC MapDtoToRegularEntity(FrontDoorProjectDto frontDoorProjectDto, Contact contact, string organisationId)
        {
            var frontDoorProjectPOC = new invln_FrontDoorProjectPOC()
            {
                invln_Name = frontDoorProjectDto.ProjectName,
                invln_ProjectSupportsHousingDeliveryinEngland = frontDoorProjectDto.ProjectSupportsHousingDeliveryinEngland,
                invln_ActivitiesinThisProject = MapNullableIntToOptionSetValueCollection(frontDoorProjectDto.ActivitiesinThisProject),
                invln_InfrastructureDelivered = MapNullableIntToOptionSetValueCollection(frontDoorProjectDto.InfrastructureDelivered),
                invln_AmountofAffordableHomes = MapNullableIntToOptionSetValue(frontDoorProjectDto.AmountofAffordableHomes),
                invln_PreviousResidentialBuildingExperience = frontDoorProjectDto.PreviousResidentialBuildingExperience,
                invln_IdentifiedSite = frontDoorProjectDto.IdentifiedSite,
                invln_GeographicFocus = MapNullableIntToOptionSetValue(frontDoorProjectDto.GeographicFocus),
                invln_Region = MapNullableIntToOptionSetValueCollection(frontDoorProjectDto.Region),
                //LocalAuthority = Guid.Empty,
                invln_NumberofHomesEnabledBuilt = frontDoorProjectDto.NumberofHomesEnabled_Built,
                invln_WouldyourprojectfailwithoutHEsupport = frontDoorProjectDto.WouldyourprojectfailwithoutHEsupport,
                invln_FundingRequired = frontDoorProjectDto.FundingRequired,
                invln_AmountofFundingRequired = MapNullableIntToOptionSetValue(frontDoorProjectDto.AmountofFundingRequired),
                invln_StartofProjectMonth = frontDoorProjectDto.StartofProjectMonth,
                invln_StartofProjectYear = frontDoorProjectDto.StartofProjectYear,
            };

            if (!string.IsNullOrEmpty(organisationId) && Guid.TryParse(organisationId, out Guid organisationGUID))
            {
                frontDoorProjectPOC.invln_AccountId = new EntityReference(Account.EntityLogicalName, organisationGUID);
            }

            if (contact != null)
            {
                frontDoorProjectPOC.invln_ContactId = contact.ToEntityReference();
            }
            return frontDoorProjectPOC;
        }

        private static OptionSetValue MapNullableIntToOptionSetValue(int? valueToMap)
        {
            if (valueToMap.HasValue)
            {
                return new OptionSetValue(valueToMap.Value);
            }
            return null;
        }

        private static OptionSetValueCollection MapNullableIntToOptionSetValueCollection(List<int> valueToMap)
        {
            if (valueToMap != null)
            {
                var osvcollection = new OptionSetValueCollection();
                foreach (var ops in valueToMap)
                {
                    osvcollection.Add(new OptionSetValue(ops));
                }
                return osvcollection;
            }
            return null;
        }
    }
}

