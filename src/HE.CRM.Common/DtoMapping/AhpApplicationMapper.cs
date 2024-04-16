using DataverseModel;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;

namespace HE.CRM.Common.DtoMapping
{
    public class AhpApplicationMapper
    {
        public static invln_scheme MapDtoToRegularEntity(AhpApplicationDto applicationDto, string contactId, string organisationId)
        {
            var applicationToReturn = new invln_scheme()
            {
                invln_schemename = applicationDto.name,
                invln_Tenure = MapNullableIntToOptionSetValue(applicationDto.tenure),
                invln_schemeinformationsectioncompletionstatus = MapNullableIntToOptionSetValue(applicationDto.schemeInformationSectionCompletionStatus),
                invln_hometypessectioncompletionstatus = MapNullableIntToOptionSetValue(applicationDto.homeTypesSectionCompletionStatus),
                invln_financialdetailssectioncompletionstatus = MapNullableIntToOptionSetValue(applicationDto.financialDetailsSectionCompletionStatus),
                invln_deliveryphasessectioncompletionstatus = MapNullableIntToOptionSetValue(applicationDto.deliveryPhasesSectionCompletionStatus),
                invln_DateSubmitted = applicationDto.dateSubmitted,
                invln_borrowingagainstrentalincome = MapNullableDecimalToMoney(applicationDto.borrowingAgainstRentalIncomeFromThisScheme),
                invln_fundingfromopenmarkethomesonthisscheme = MapNullableDecimalToMoney(applicationDto.fundingFromOpenMarketHomesOnThisScheme),
                invln_fundingfromopenmarkethomesnotonthisscheme = MapNullableDecimalToMoney(applicationDto.fundingFromOpenMarketHomesNotOnThisScheme),
                invln_fundinggeneratedfromothersources = MapNullableDecimalToMoney(applicationDto.fundingGeneratedFromOtherSources),
                invln_recycledcapitalgrantfund = MapNullableDecimalToMoney(applicationDto.recycledCapitalGrantFund),
                invln_transfervalue = MapNullableDecimalToMoney(applicationDto.transferValue),
                invln_totalinitialsalesincome = MapNullableDecimalToMoney(applicationDto.totalInitialSalesIncome),
                invln_othercapitalsources = MapNullableDecimalToMoney(applicationDto.otherCapitalSources),
                invln_fundingrequired = MapNullableDecimalToMoney(applicationDto.fundingRequested),
                invln_noofhomes = applicationDto.noOfHomes,
                invln_affordabilityevidence = applicationDto.affordabilityEvidence,
                invln_discussionswithlocalstakeholders = applicationDto.discussionsWithLocalStakeholders,
                invln_meetinglocalhousingneed = applicationDto.meetingLocalHousingNeed,
                invln_meetinglocalpriorities = applicationDto.meetingLocalProrities,
                invln_reducingenvironmentalimpact = applicationDto.reducingEnvironmentalImpact,
                invln_sharedownershipsalesrisk = applicationDto.sharedOwnershipSalesRisk,
                invln_publicland = applicationDto.isPublicLand,
                invln_currentlandvalue = MapNullableDecimalToMoney(applicationDto.currentLandValue),
                invln_expectedoncosts = MapNullableDecimalToMoney(applicationDto.expectedOnCosts),
                invln_expectedonworks = MapNullableDecimalToMoney(applicationDto.expectedOnWorks),
                invln_expectedacquisitioncost = MapNullableDecimalToMoney(applicationDto.expectedAcquisitionCost),
                invln_actualacquisitioncost = MapNullableDecimalToMoney(applicationDto.actualAcquisitionCost),
                invln_ownresources = MapNullableDecimalToMoney(applicationDto.ownResources),
                //home Information
                invln_grantsfromcountycouncil = MapNullableDecimalToMoney(applicationDto.howMuchReceivedFromCountyCouncil),
                invln_grantsfromdhscextracare = MapNullableDecimalToMoney(applicationDto.howMuchReceivedFromDhscExtraCareFunding),
                invln_grantsfromlocalauthority1 = MapNullableDecimalToMoney(applicationDto.howMuchReceivedFromLocalAuthority1),
                invln_grantsfromlocalauthority2 = MapNullableDecimalToMoney(applicationDto.howMuchReceivedFromLocalAuthority2),
                invln_grantsfromsocialservices = MapNullableDecimalToMoney(applicationDto.howMuchReceivedFromSocialServices),
                invln_grantsfromdhscnhsorotherhealth = MapNullableDecimalToMoney(applicationDto.howMuchReceivedFromDepartmentOfHealth),
                invln_grantsfromthelottery = MapNullableDecimalToMoney(applicationDto.howMuchReceivedFromLotteryFunding),
                invln_grantsfromotherpublicbodies = MapNullableDecimalToMoney(applicationDto.howMuchReceivedFromOtherPublicBodies),
            };
            if (applicationDto.id != null && Guid.TryParse(applicationDto.id, out var applicationId))
            {
                applicationToReturn.Id = applicationId;
            }
            if (Guid.TryParse(contactId, out var contactGuid))
            {
                applicationToReturn.invln_contactid = new EntityReference(Contact.EntityLogicalName, contactGuid);
            }
            if (Guid.TryParse(organisationId, out var organisationGuid))
            {
                applicationToReturn.invln_organisationid = new EntityReference(Account.EntityLogicalName, organisationGuid);
            }
            if (Guid.TryParse(applicationDto.siteId, out var siteGuid))
            {
                applicationToReturn.invln_Site = new EntityReference(invln_Sites.EntityLogicalName, siteGuid);
            }
            if (Guid.TryParse(applicationDto.programmeId, out var programmeId))
            {
                applicationToReturn.invln_programmelookup = new EntityReference(invln_programme.EntityLogicalName, programmeId);
            }

            return applicationToReturn;
        }

        public static AhpApplicationDto MapRegularEntityToDto(invln_scheme application, string contactExternalId = null)
        {
            var applicationDtoToReturn = new AhpApplicationDto()
            {
                name = application.invln_schemename,
                tenure = application.invln_Tenure?.Value,
                schemeInformationSectionCompletionStatus = application.invln_schemeinformationsectioncompletionstatus?.Value,
                homeTypesSectionCompletionStatus = application.invln_hometypessectioncompletionstatus?.Value,
                financialDetailsSectionCompletionStatus = application.invln_financialdetailssectioncompletionstatus?.Value,
                deliveryPhasesSectionCompletionStatus = application.invln_deliveryphasessectioncompletionstatus?.Value,
                dateSubmitted = application.invln_DateSubmitted,
                borrowingAgainstRentalIncomeFromThisScheme = application.invln_borrowingagainstrentalincome?.Value,
                fundingFromOpenMarketHomesOnThisScheme = application.invln_fundingfromopenmarkethomesonthisscheme?.Value,
                fundingFromOpenMarketHomesNotOnThisScheme = application.invln_fundingfromopenmarkethomesnotonthisscheme?.Value,
                fundingGeneratedFromOtherSources = application.invln_fundinggeneratedfromothersources?.Value,
                recycledCapitalGrantFund = application.invln_recycledcapitalgrantfund?.Value,
                transferValue = application.invln_transfervalue?.Value,
                totalInitialSalesIncome = application.invln_totalinitialsalesincome?.Value,
                otherCapitalSources = application.invln_othercapitalsources?.Value,
                fundingRequested = application.invln_fundingrequired?.Value,
                noOfHomes = application.invln_noofhomes,
                affordabilityEvidence = application.invln_affordabilityevidence,
                discussionsWithLocalStakeholders = application.invln_discussionswithlocalstakeholders,
                meetingLocalHousingNeed = application.invln_meetinglocalhousingneed,
                meetingLocalProrities = application.invln_meetinglocalpriorities,
                reducingEnvironmentalImpact = application.invln_reducingenvironmentalimpact,
                sharedOwnershipSalesRisk = application.invln_sharedownershipsalesrisk,
                isPublicLand = application.invln_publicland,
                currentLandValue = application.invln_currentlandvalue?.Value,
                expectedOnCosts = application.invln_expectedoncosts?.Value,
                expectedOnWorks = application.invln_expectedonworks?.Value,
                expectedAcquisitionCost = application.invln_expectedacquisitioncost?.Value,
                actualAcquisitionCost = application.invln_actualacquisitioncost?.Value,
                ownResources = application.invln_ownresources?.Value,
                lastExternalModificationOn = application.invln_lastexternalmodificationon,
                howMuchReceivedFromCountyCouncil = application.invln_grantsfromcountycouncil?.Value,
                howMuchReceivedFromDhscExtraCareFunding = application.invln_grantsfromdhscextracare?.Value,
                howMuchReceivedFromLocalAuthority1 = application.invln_grantsfromlocalauthority1?.Value,
                howMuchReceivedFromLocalAuthority2 = application.invln_grantsfromlocalauthority2?.Value,
                howMuchReceivedFromSocialServices = application.invln_grantsfromsocialservices?.Value,
                howMuchReceivedFromDepartmentOfHealth = application.invln_grantsfromdhscnhsorotherhealth?.Value,
                howMuchReceivedFromLotteryFunding = application.invln_grantsfromthelottery?.Value,
                howMuchReceivedFromOtherPublicBodies = application.invln_grantsfromotherpublicbodies?.Value,
                referenceNumber = application.invln_applicationid,
                applicationStatus = application.invln_ExternalStatus?.Value,
                contactExternalId = contactExternalId,

            };
            if (application.invln_programmelookup != null)
            {
                applicationDtoToReturn.programmeId = application.invln_programmelookup.Id.ToString();
            }
            if (application.Id != null)
            {
                applicationDtoToReturn.id = application.Id.ToString();
            }
            if (application.invln_organisationid != null)
            {
                applicationDtoToReturn.organisationId = application.invln_organisationid.Id.ToString();
            }
            if (application.invln_contactid != null)
            {
                applicationDtoToReturn.contactId = application.invln_contactid.Id.ToString();
            }
            if (application.invln_Site != null)
            {
                applicationDtoToReturn.siteId = application.invln_Site.Id.ToString();
            }
            return applicationDtoToReturn;
        }

        private static OptionSetValue MapNullableIntToOptionSetValue(int? valueToMap)
        {
            if (valueToMap.HasValue)
            {
                return new OptionSetValue(valueToMap.Value);
            }
            return null;
        }

        private static Money MapNullableDecimalToMoney(decimal? valueToMap)
        {
            if (valueToMap.HasValue)
            {
                return new Money(valueToMap.Value);
            }
            return null;
        }
    }
}
