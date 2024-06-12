using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.FrontDoor.Domain.Project.Storage.Api.Contract;

namespace HE.Investments.FrontDoor.Domain.Project.Storage.Api.Mappers;

internal static class FrontDoorProjectContactMapper
{
    public static UserAccountDto Map(FrontDoorProjectContact response)
    {
        return new UserAccountDto
        {
            AccountId = response.AccountId,
            ContactEmail = response.ContactEmail,
            ContactExternalId = "auth0|65800f41617b364da56913f9", // TODO: AB#98936 map when API will return external Id
            ContactFirstName = response.ContactFirstName,
            ContactLastName = response.ContactLastName,
            ContactTelephoneNumber = response.ContactTelephoneNumber ?? string.Empty,
        };
    }
}
