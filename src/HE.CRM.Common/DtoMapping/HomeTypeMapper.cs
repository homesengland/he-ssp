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
            };
            if (homeType.Id != null)
            {
                homeTypeDto.id = homeType.Id.ToString();
            }

            if (homeType.invln_application?.Id != null)
            {
                homeTypeDto.applicationId = homeType.invln_application.Id.ToString();
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
            };
            if (homeTypeDto.id != null)
            {
                homeType.Id = new Guid(homeTypeDto.id);
            }

            if (Guid.TryParse(applicationId ?? homeTypeDto.applicationId, out var applicationGuid))
            {
                homeType.invln_application = new EntityReference(invln_scheme.EntityLogicalName, applicationGuid);
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
