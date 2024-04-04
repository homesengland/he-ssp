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

        public static FrontDoorProjectDto MapRegularEntityToDto(invln_FrontDoorProjectPOC project, Contact contact = null, invln_localauthority localauthority = null)
        {
            var frontDoorProjectDto = new FrontDoorProjectDto()
            {
                ProjectId = project.Id.ToString(),
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
                LocalAuthority = project.invln_LocalAuthorityId != null ? project.invln_LocalAuthorityId.Id.ToString() : string.Empty,
                LocalAuthorityName = project.invln_LocalAuthorityId != null ? project.invln_LocalAuthorityId.Name : string.Empty,
                LocalAuthorityCode = localauthority.invln_onscode != null ? localauthority.invln_onscode : string.Empty,
                NumberofHomesEnabledBuilt = project.invln_NumberofHomesEnabledBuilt,
                WouldyourprojectfailwithoutHEsupport = project.invln_WouldyourprojectfailwithoutHEsupport,
                FundingRequired = project.invln_FundingRequired,
                AmountofFundingRequired = project.invln_AmountofFundingRequired?.Value,
                StartofProjectMonth = project.invln_StartofProjectMonth,
                StartofProjectYear = project.invln_StartofProjectYear,
                CreatedOn = project.CreatedOn,
                IntentiontoMakeaProfit = project.invln_IntentiontoMakeaProfit,
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
                frontDoorProjectDto.externalId = contact.invln_externalid;
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
                invln_NumberofHomesEnabledBuilt = frontDoorProjectDto.NumberofHomesEnabledBuilt,
                invln_WouldyourprojectfailwithoutHEsupport = frontDoorProjectDto.WouldyourprojectfailwithoutHEsupport,
                invln_FundingRequired = frontDoorProjectDto.FundingRequired,
                invln_AmountofFundingRequired = MapNullableIntToOptionSetValue(frontDoorProjectDto.AmountofFundingRequired),
                invln_StartofProjectMonth = frontDoorProjectDto.StartofProjectMonth,
                invln_StartofProjectYear = frontDoorProjectDto.StartofProjectYear,
                invln_IntentiontoMakeaProfit = frontDoorProjectDto.IntentiontoMakeaProfit,
            };

            if (!string.IsNullOrEmpty(organisationId) && Guid.TryParse(organisationId, out Guid organisationGUID))
            {
                frontDoorProjectPOC.invln_AccountId = new EntityReference(Account.EntityLogicalName, organisationGUID);
            }

            if (contact != null)
            {
                frontDoorProjectPOC.invln_ContactId = contact.ToEntityReference();
            }

            if (!string.IsNullOrEmpty(frontDoorProjectDto.LocalAuthority) && Guid.TryParse(frontDoorProjectDto.LocalAuthority, out Guid localAuthorityGUID) && localAuthorityGUID != Guid.Empty)
            {
                frontDoorProjectPOC.invln_LocalAuthorityId = new EntityReference(invln_localauthority.EntityLogicalName, localAuthorityGUID);
            }
            else
            {
                frontDoorProjectPOC.invln_LocalAuthorityId = null;
            }


            return frontDoorProjectPOC;
        }



        public static FrontDoorProjectDto MapHeProjectEntityToDto(he_Pipeline project, Contact contact = null, he_LocalAuthority localauthority = null)
        {
            var frontDoorProjectDto = new FrontDoorProjectDto()
            {
                ProjectId = project.Id.ToString(),
                OrganisationId = project.he_Account != null ? project.he_Account.Id : Guid.Empty,
                ProjectName = project.he_ProjectName,
                ProjectSupportsHousingDeliveryinEngland = project.he_housingdeliveryinengland,
                ActivitiesinThisProject = new List<int>(),
                InfrastructureDelivered = new List<int>(),
                AmountofAffordableHomes = project.he_amountofaffordablehomes?.Value,
                PreviousResidentialBuildingExperience = project.he_previousresidentialbuildingexperience,
                IdentifiedSite = project.he_doyouhaveanidentifiedsite,
                GeographicFocus = project.he_whatisthegeographicfocusoftheproject?.Value,
                Region = new List<int>(),
                LocalAuthority = project.he_projectbelocated != null ? project.he_projectbelocated.Id.ToString() : string.Empty,
                LocalAuthorityName = project.he_projectbelocated != null ? project.he_projectbelocated.Name : string.Empty,
                LocalAuthorityCode = localauthority.he_GSSCode != null ? localauthority.he_GSSCode : string.Empty,
                NumberofHomesEnabledBuilt = project.he_howmanyhomeswillyourprojectenable,
                WouldyourprojectfailwithoutHEsupport = project.he_projectprogressmoreslowly,
                FundingRequired = project.he_doyourequirefundingforyourproject,
                AmountofFundingRequired = project.he_howmuchfundingdoyourequired?.Value,
                StartofProjectMonth = project.he_startofprojectmonth,
                StartofProjectYear = project.he_startofprojectyear,
                CreatedOn = project.CreatedOn,
                IntentiontoMakeaProfit = project.he_intentiontomakeaprofit,
            };

            if (project.he_activitiesinthisproject != null && project.he_activitiesinthisproject.Any())
            {
                foreach (var activitiinThisProjec in project.he_activitiesinthisproject)
                {
                    frontDoorProjectDto.ActivitiesinThisProject.Add(activitiinThisProjec.Value);
                }
            }

            if (project.he_he_infrastructuredoesyourprojectdeliver != null && project.he_he_infrastructuredoesyourprojectdeliver.Any())
            {
                foreach (var infrastructureDelivered in project.he_he_infrastructuredoesyourprojectdeliver)
                {
                    frontDoorProjectDto.InfrastructureDelivered.Add(infrastructureDelivered.Value);
                }
            }
            if (project.he_regionlocation != null && project.he_regionlocation.Any())
            {
                foreach (var region in project.he_regionlocation)
                {
                    frontDoorProjectDto.Region.Add(region.Value);
                }
            }

            if (contact != null)
            {
                frontDoorProjectDto.externalId = contact.invln_externalid;
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

        public static he_Pipeline MapDtoToProjectEntity(FrontDoorProjectDto frontDoorProjectDto, Contact contact, string organisationId)
        {
            var Pipeline = new he_Pipeline()
            {
                he_Name = frontDoorProjectDto.ProjectName,
                he_ProjectName = frontDoorProjectDto.ProjectName,
                he_housingdeliveryinengland = frontDoorProjectDto.ProjectSupportsHousingDeliveryinEngland,
                he_activitiesinthisproject = MapNullableIntToOptionSetValueCollection(frontDoorProjectDto.ActivitiesinThisProject),
                he_he_infrastructuredoesyourprojectdeliver = MapNullableIntToOptionSetValueCollection(frontDoorProjectDto.InfrastructureDelivered),
                he_amountofaffordablehomes = MapNullableIntToOptionSetValue(frontDoorProjectDto.AmountofAffordableHomes),
                he_previousresidentialbuildingexperience = frontDoorProjectDto.PreviousResidentialBuildingExperience,
                he_doyouhaveanidentifiedsite = frontDoorProjectDto.IdentifiedSite,
                he_whatisthegeographicfocusoftheproject = MapNullableIntToOptionSetValue(frontDoorProjectDto.GeographicFocus),
                he_regionlocation = MapNullableIntToOptionSetValueCollection(frontDoorProjectDto.Region),
                he_howmanyhomeswillyourprojectenable = frontDoorProjectDto.NumberofHomesEnabledBuilt,
                he_projectprogressmoreslowly = frontDoorProjectDto.WouldyourprojectfailwithoutHEsupport,
                he_doyourequirefundingforyourproject = frontDoorProjectDto.FundingRequired,
                he_howmuchfundingdoyourequired = MapNullableIntToOptionSetValue(frontDoorProjectDto.AmountofFundingRequired),
                he_startofprojectmonth = frontDoorProjectDto.StartofProjectMonth,
                he_startofprojectyear = frontDoorProjectDto.StartofProjectYear,
                he_intentiontomakeaprofit = frontDoorProjectDto.IntentiontoMakeaProfit,
                he_Projecttype = MapNullableIntToOptionSetValue(((int)he_Pipeline_he_Projecttype.Site)),
            };

            if (!string.IsNullOrEmpty(organisationId) && Guid.TryParse(organisationId, out Guid organisationGUID))
            {
                Pipeline.he_Account = new EntityReference(Account.EntityLogicalName, organisationGUID);
            }

            if (contact != null)
            {
                Pipeline.he_portalowner = contact.ToEntityReference();
            }

            if (!string.IsNullOrEmpty(frontDoorProjectDto.LocalAuthority) && Guid.TryParse(frontDoorProjectDto.LocalAuthority, out Guid localAuthorityGUID) && localAuthorityGUID != Guid.Empty)
            {
                Pipeline.he_projectbelocated = new EntityReference(he_LocalAuthority.EntityLogicalName, localAuthorityGUID);
            }
            else
            {
                Pipeline.he_projectbelocated = null;
            }

            return Pipeline;
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

