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
    }
}
