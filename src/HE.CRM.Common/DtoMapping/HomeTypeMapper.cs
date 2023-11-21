using System;
using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Common.DtoMapping
{
    public class HomeTypeMapper
    {
        public static HomeTypeDto MapRegularEntityToDto(invln_HomeType homeType)
        {
            var homeTypeDto = new HomeTypeDto()
            {
                homeTypeName = homeType.invln_hometypename,
                housingType = homeType.invln_typeofhousing?.Value,
                numberOfHomes = homeType.invln_numberofhomeshometype,
                housingTypeForOlderPeople = homeType.invln_typeofolderpeopleshousing?.Value,
                housingTypeForVulnerable = homeType.invln_typeofhousingfordisabledvulnerablepeople?.Value,
                clientGroup = homeType.invln_clientgroup?.Value,
                designPrinciples = new List<int>(),
                localComissioningBodies = homeType.invln_localcommissioningbodiesconsulted,
                shortStayAccommodation = homeType.invln_homesusedforshortstay,
                revenueFunding = homeType.invln_revenuefunding?.Value,
                moveOnArrangementsForNoRevenueFunding = homeType.invln_moveonarrangementsfornorevenuefunding,
                fundingSources = new List<int>(),
                moveOnArrangementsForRevenueFunding = homeType.invln_Moveonarrangementsforrevenuefunding,
                exitPlan = homeType.invln_supportedhousingexitplan,
                typologyLocationAndDesign = homeType.invln_typologylocationanddesing,
                numberOfBedrooms = homeType.invln_numberofbedrooms,
                maxOccupancy = homeType.invln_maxoccupancy,
                numberOfStoreys = homeType.invln_numberofstoreys,
            };
            if (homeType.Id != null)
            {
                homeTypeDto.id = homeType.Id.ToString();
            }

            if (homeType.invln_application?.Id != null)
            {
                homeTypeDto.applicationId = homeType.invln_application.Id.ToString();
            }

            if (homeType.invln_revenuefundingsources != null && homeType.invln_revenuefundingsources.Any())
            {
                foreach (var revenueFundingSource in homeType.invln_revenuefundingsources)
                {
                    homeTypeDto.fundingSources.Add(revenueFundingSource.Value);
                }
            }

            if (homeType.invln_happiprinciples != null && homeType.invln_happiprinciples.Any())
            {
                foreach (var principle in homeType.invln_happiprinciples)
                {
                    homeTypeDto.designPrinciples.Add(principle.Value);
                }
            }
            return homeTypeDto;
        }

        public static invln_HomeType MapDtoToRegularEntity(HomeTypeDto homeTypeDto, string applicationId = null)
        {
            var homeType = new invln_HomeType()
            {
                invln_hometypename = homeTypeDto.homeTypeName,
                invln_typeofhousing = MapNullableIntToOptionSetValue(homeTypeDto.housingType),
                invln_numberofhomeshometype = homeTypeDto.numberOfHomes,
                invln_typeofolderpeopleshousing = MapNullableIntToOptionSetValue(homeTypeDto.housingTypeForOlderPeople),
                invln_typeofhousingfordisabledvulnerablepeople = MapNullableIntToOptionSetValue(homeTypeDto.housingTypeForVulnerable),
                invln_clientgroup = MapNullableIntToOptionSetValue(homeTypeDto.clientGroup),
                invln_happiprinciples = new OptionSetValueCollection(),
                invln_localcommissioningbodiesconsulted = homeTypeDto.localComissioningBodies,
                invln_homesusedforshortstay = homeTypeDto.shortStayAccommodation,
                invln_revenuefunding = MapNullableIntToOptionSetValue(homeTypeDto.revenueFunding),
                invln_moveonarrangementsfornorevenuefunding = homeTypeDto.moveOnArrangementsForNoRevenueFunding,
                invln_revenuefundingsources = new OptionSetValueCollection(),
                invln_Moveonarrangementsforrevenuefunding = homeTypeDto.moveOnArrangementsForRevenueFunding,
                invln_supportedhousingexitplan = homeTypeDto.exitPlan,
                invln_typologylocationanddesing = homeTypeDto.typologyLocationAndDesign,
                invln_numberofbedrooms = homeTypeDto.numberOfBedrooms,
                invln_maxoccupancy = homeTypeDto.maxOccupancy,
                invln_numberofstoreys = homeTypeDto.numberOfStoreys,
            };
            if (homeTypeDto.id != null)
            {
                homeType.Id = new Guid(homeTypeDto.id);
            }

            if (Guid.TryParse(applicationId ?? homeTypeDto.applicationId, out var applicationGuid))
            {
                homeType.invln_application = new EntityReference(invln_scheme.EntityLogicalName, applicationGuid);
            }

            if (homeTypeDto.fundingSources != null && homeTypeDto.fundingSources.Any())
            {
                foreach (var fundingSource in homeTypeDto.fundingSources)
                {
                    homeType.invln_revenuefundingsources.Add(new OptionSetValue(fundingSource));
                }
            }

            if (homeTypeDto.designPrinciples != null && homeTypeDto.designPrinciples.Any())
            {
                foreach (var principle in homeTypeDto.designPrinciples)
                {
                    homeType.invln_happiprinciples.Add(new OptionSetValue(principle));
                }
            }

            return homeType;
        }

        private static OptionSetValue MapNullableIntToOptionSetValue(int? valueToMap)
        {
            if (valueToMap.HasValue)
            {
                return new OptionSetValue(valueToMap.Value);
            }
            return null;
        }
    }
}
