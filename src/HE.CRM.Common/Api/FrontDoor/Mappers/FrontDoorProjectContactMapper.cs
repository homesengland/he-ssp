using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Api.FrontDoor.Contract;

namespace HE.CRM.Common.Api.FrontDoor.Mappers
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
