using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Plugins.Models.FrontDoor.Contract;

namespace HE.CRM.Plugins.Models.Frontdoor.Mappers
{
    public static class FrontDoorProjectContactMapper
    {
        public static UserAccountDto Map(FrontDoorProjectContact response)
        {
            return new UserAccountDto
            {
                AccountId = response.AccountId,
                ContactEmail = response.ContactEmail,
                ContactExternalId = null,
                ContactFirstName = response.ContactFirstName,
                ContactLastName = response.ContactLastName,
                ContactTelephoneNumber = response.ContactTelephoneNumber ?? string.Empty,
            };
        }
    }
}
