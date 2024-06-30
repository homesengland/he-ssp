using System;
using System.Collections.Generic;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Api.FrontDoor.Contract;

namespace HE.CRM.Common.Api.FrontDoor.Mappers
{
    public static class FrontDoorProjectContactMapper
    {
        public static UserAccountDto Map(FrontDoorProjectContact response, Dictionary<Guid, string> contactsExternalIdMap)
        {
            if (!contactsExternalIdMap.TryGetValue(response.ContactId, out var contactExternalId))
            {
            }

            return new UserAccountDto
            {
                AccountId = response.AccountId,
                ContactEmail = response.ContactEmail,
                ContactExternalId = contactExternalId,
                ContactFirstName = response.ContactFirstName,
                ContactLastName = response.ContactLastName,
                ContactTelephoneNumber = response.ContactTelephoneNumber ?? string.Empty,
            };
        }
    }
}
