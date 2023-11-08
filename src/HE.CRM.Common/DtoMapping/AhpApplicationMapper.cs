using DataverseModel;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;

namespace HE.CRM.Common.DtoMapping
{
    public class AhpApplicationMapper
    {
        public static invln_scheme MapDtoToRegularEntity(AhpApplicationDto applicationDto)
        {
            var applicationToReturn = new invln_scheme()
            {
                invln_schemename = applicationDto.name,
                invln_Tenure = MapNullableIntToOptionSetValue(applicationDto.tenure),
            };
            if (applicationDto.id != null && Guid.TryParse(applicationDto.id, out var applicationId))
            {
                applicationToReturn.Id = applicationId;
            }
            return applicationToReturn;
        }

        public static AhpApplicationDto MapRegularEntityToDto(invln_scheme application)
        {
            var applicationDtoToReturn = new AhpApplicationDto()
            {
                name = application.invln_schemename,
                tenure = application.invln_Tenure?.Value,
            };
            if(application.Id != null)
            {
                applicationDtoToReturn.id = application.Id.ToString();
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
    }
}
