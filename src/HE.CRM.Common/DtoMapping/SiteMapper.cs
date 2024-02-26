using System;
using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Extensions.Entities;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Common.DtoMapping
{
    public static class SiteMapper
    {
        public static SiteDto ToDto(invln_Sites entity)
        {
            return new SiteDto
            {
                id = entity.invln_SitesId?.ToString(),
                name = entity.invln_sitename,
                section106 = new Section106Dto
                {
                    isAgreement106 = entity.invln_s106agreementinplace,
                    areContributionsForAffordableHomes = entity.invln_developercontributionsforah,
                    isAffordableHousing100 = entity.invln_siteis100affordable,
                    areAdditionalHomes = entity.invln_homesintheapplicationareadditional,
                    areRestrictionsOrObligations = entity.invln_anyrestrictionsinthes106,
                    localAuthorityConfirmation = entity.invln_localauthorityconfirmationofadditionality,
                },
                localAuthority = new SiteLocalAuthority
                {
                   id = entity.invln_LocalAuthority?.Id.ToString(),
                   name = entity.GetAttributeValue<AliasedValue>("invln_ahglocalauthorities1.invln_gsscode")?.Value.ToString(),
                },
                planningDetails = new PlanningDetailsDto
                {
                    planningStatus = entity.invln_planningstatus?.Value,
                    referenceNumber = entity.invln_planningreferencenumber,
                    detailedPlanningApprovalDate = entity.invln_detailedplanningapprovaldate,
                    requiredFurtherSteps = entity.invln_furtherstepsrequired,
                    applicationForDetailedPlanningSubmittedDate = entity.invln_applicationfordetailedplanningsubmitted,
                    expectedPlanningApprovalDate = entity.invln_expectedplanningapproval,
                    outlinePlanningApprovalDate = entity.invln_outlineplanningapprovaldate,
                    planningSubmissionDate = entity.invln_planningsubmissiondate,
                },
                nationalDesignGuidePriorities = entity.invln_nationaldesignguideelements.ToIntValueList(),
                buildingForHealthyLife = entity.invln_assessedforbhl?.Value,
                numberOfGreenLights = entity.invln_bhlgreentrafficlights,
                landStatus = entity.invln_landstatus?.Value,
                tenderingDetails = new TenderingDetailsDto
                {
                    tenderingStatus = entity.invln_workstenderingstatus?.Value,
                    contractorName = entity.invln_maincontractorname,
                    isSme = entity.invln_sme,
                    isIntentionToWorkWithSme = entity.invln_intentiontoworkwithsme,
                },
                strategicSiteDetails =
                    new StrategicSiteDetailsDto { isStrategicSite = entity.invln_strategicsite, name = entity.invln_StrategicSiteN, },
                siteTypeDetails =
                    new SiteTypeDetailsDto
                    {
                        siteType = entity.invln_TypeofSite?.Value, isGreenBelt = entity.invln_greenbelt, isRegenerationSite = entity.invln_regensite,
                    },
                siteUseDetails =
                    new SiteUseDetailsDto
                    {
                        isPartOfStreetFrontInfill = entity.invln_streetfrontinfill,
                        isForTravellerPitchSite = entity.invln_travellerpitchsite,
                        travellerPitchSiteType = entity.invln_travellerpitchsitetype?.Value,
                    },
                ruralDetails = new RuralDetailsDto
                {
                    isRuralClassification = entity.invln_Ruralclassification, isRuralExceptionSite = entity.invln_RuralExceptionSite,
                },
                environmentalImpact = entity.invln_ActionstoReduce,
            };
        }

        public static invln_Sites ToEntity(SiteDto dto, string fieldsToSet, invln_AHGLocalAuthorities localAuthorities)
        {
            var site = new invln_Sites();

            if (!string.IsNullOrWhiteSpace(dto.id) && Guid.TryParse(dto.id, out var siteId))
            {
                site.Id = siteId;
            }

            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_sitename), dto.name);
            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.StateCode), dto.name);

            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_s106agreementinplace), dto.section106.isAgreement106);
            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_developercontributionsforah), dto.section106.areContributionsForAffordableHomes);
            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_siteis100affordable), dto.section106.isAffordableHousing100);
            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_homesintheapplicationareadditional), dto.section106.areAdditionalHomes);
            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_anyrestrictionsinthes106), dto.section106.areRestrictionsOrObligations);
            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_localauthorityconfirmationofadditionality), dto.section106.localAuthorityConfirmation);

            if (localAuthorities != null)
            {
                SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_LocalAuthority), new EntityReference(nameof(invln_AHGLocalAuthorities).ToLower(), localAuthorities.Id));
            }

            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_planningstatus), TryCreateOptionSetValue(dto.planningDetails.planningStatus));
            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_planningreferencenumber), dto.planningDetails.referenceNumber);
            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_detailedplanningapprovaldate), dto.planningDetails.detailedPlanningApprovalDate);
            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_furtherstepsrequired), dto.planningDetails.requiredFurtherSteps);
            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_applicationfordetailedplanningsubmitted), dto.planningDetails.applicationForDetailedPlanningSubmittedDate);
            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_expectedplanningapproval), dto.planningDetails.expectedPlanningApprovalDate);
            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_outlineplanningapprovaldate), dto.planningDetails.outlinePlanningApprovalDate);
            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_planningsubmissiondate), dto.planningDetails.planningSubmissionDate);

            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_nationaldesignguideelements), CreateOptionSetValueCollection(dto.nationalDesignGuidePriorities));

            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_assessedforbhl), TryCreateOptionSetValue(dto.buildingForHealthyLife));
            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_bhlgreentrafficlights), dto.numberOfGreenLights);

            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_landstatus), TryCreateOptionSetValue(dto.landStatus));

            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_workstenderingstatus), TryCreateOptionSetValue(dto.tenderingDetails.tenderingStatus));
            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_maincontractorname), dto.tenderingDetails.contractorName);
            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_sme), dto.tenderingDetails.isSme);
            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_intentiontoworkwithsme), dto.tenderingDetails.isIntentionToWorkWithSme);

            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_strategicsite), dto.strategicSiteDetails.isStrategicSite);
            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_StrategicSiteN), dto.strategicSiteDetails.name);

            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_TypeofSite), TryCreateOptionSetValue(dto.siteTypeDetails.siteType));
            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_greenbelt), dto.siteTypeDetails.isGreenBelt);
            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_regensite), dto.siteTypeDetails.isRegenerationSite);

            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_streetfrontinfill), dto.siteUseDetails.isPartOfStreetFrontInfill);
            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_travellerpitchsite), dto.siteUseDetails.isForTravellerPitchSite);
            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_travellerpitchsitetype), TryCreateOptionSetValue(dto.siteUseDetails.travellerPitchSiteType));

            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_Ruralclassification), dto.ruralDetails.isRuralClassification);
            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_RuralExceptionSite), dto.ruralDetails.isRuralExceptionSite);

            SetFieldValue(site, fieldsToSet, nameof(invln_Sites.invln_ActionstoReduce), dto.environmentalImpact);

            return site;
        }

        private static void SetFieldValue<T>(invln_Sites site, string fieldsToSet, string fieldName, T value)
        {
            if (fieldsToSet.Contains(fieldName.ToLower()))
            {
                site[fieldName.ToLower()] = value;
            }
        }

        private static OptionSetValue TryCreateOptionSetValue(int? value)
        {
            return value != null ? new OptionSetValue((int)value) : null;
        }

        private static OptionSetValueCollection CreateOptionSetValueCollection(IList<int> dto)
        {
            return new OptionSetValueCollection(dto.Select(x => new OptionSetValue(x)).ToList());
        }
    }
}
