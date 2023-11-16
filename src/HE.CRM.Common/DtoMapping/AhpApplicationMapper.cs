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
                invln_borrowingagainstrentalincome = MapNullableDecimalToMoney(applicationDto.borrowingAgainstRentalIncomeFromThisScheme),
                invln_fundingfromopenmarkethomesonthisscheme = MapNullableDecimalToMoney(applicationDto.fundingFromOpenMarketHomesOnThisScheme),
                invln_fundingfromopenmarkethomesnotonthisscheme = MapNullableDecimalToMoney(applicationDto.fundingFromOpenMarketHomesNotOnThisScheme),
                //invln_funding = MapNullableDecimalToMoney(applicationDto.fundingGeneratedFromOtherSources),
                invln_recycledcapitalgrantfund = MapNullableDecimalToMoney(applicationDto.recycledCapitalGrantFund),
                //invln_transfer = MapNullableDecimalToMoney(applicationDto.transferValue),
                //invln_totalinitial = MapNullableDecimalToMoney(applicationDto.totalInitialSalesIncome),
                //invln_other = MapNullableDecimalToMoney(applicationDto.otherCapitalSources),
                invln_fundingrequired = MapNullableDecimalToMoney(applicationDto.fundingRequested),
                invln_noofhomes = applicationDto.noOfHomes,
                invln_affordabilityevidence = applicationDto.affordabilityEvidence,
                invln_discussionswithlocalstakeholders = applicationDto.discussionsWithLocalStakeholders,
                invln_meetinglocalhousingneed = applicationDto.meetingLocalHousingNeed,
                invln_meetinglocalpriorities = applicationDto.meetingLocalProrities,
                invln_reducingenvironmentalimpact = applicationDto.reducingEnvironmentalImpact,
                invln_sharedownershipsalesrisk = applicationDto.sharedOwnershipSalesRisk


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
            return applicationToReturn;
        }

        public static AhpApplicationDto MapRegularEntityToDto(invln_scheme application)
        {
            var applicationDtoToReturn = new AhpApplicationDto()
            {
                name = application.invln_schemename,
                tenure = application.invln_Tenure?.Value,
                schemeInformationSectionCompletionStatus = application.invln_schemeinformationsectioncompletionstatus?.Value,
                homeTypesSectionCompletionStatus = application.invln_hometypessectioncompletionstatus?.Value,
                financialDetailsSectionCompletionStatus = application.invln_financialdetailssectioncompletionstatus?.Value,
                deliveryPhasesSectionCompletionStatus = application.invln_deliveryphasessectioncompletionstatus?.Value,
                borrowingAgainstRentalIncomeFromThisScheme = application.invln_borrowingagainstrentalincome?.Value,
                fundingFromOpenMarketHomesOnThisScheme = application.invln_fundingfromopenmarkethomesonthisscheme?.Value,
                fundingFromOpenMarketHomesNotOnThisScheme = application.invln_fundingfromopenmarkethomesnotonthisscheme?.Value,
                //fundingGeneratedFromOtherSources = application.?.Value,
                recycledCapitalGrantFund = application.invln_recycledcapitalgrantfund?.Value,
                //transferValue = application.?.Value,
                //totalInitialSalesIncome = application.?.Value,
                //otherCapitalSources = application.?.Value,
                fundingRequested = application.invln_fundingrequired?.Value,
                noOfHomes = application.invln_noofhomes,
                affordabilityEvidence = application.invln_affordabilityevidence,
                discussionsWithLocalStakeholders = application.invln_discussionswithlocalstakeholders,
                meetingLocalHousingNeed = application.invln_meetinglocalhousingneed,
                meetingLocalProrities = application.invln_meetinglocalpriorities,
                reducingEnvironmentalImpact = application.invln_reducingenvironmentalimpact,
                sharedOwnershipSalesRisk = application.invln_sharedownershipsalesrisk,
            };
            if (application.Id != null)
            {
                applicationDtoToReturn.id = application.Id.ToString();
            }
            if (application.invln_organisationid != null)
            {
                applicationDtoToReturn.organisationId = application.invln_organisationid.ToString();
            }
            if (application.invln_contactid != null)
            {
                applicationDtoToReturn.contactId = application.invln_contactid.ToString();
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
