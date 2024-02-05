using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.Account.Shared.Tests.User.TestData;

public static class ContactDtoTestData
{
    public static readonly ContactDto ContactDto = new()
    {
        contactId = "UserOne",
        firstName = "Jacob",
        lastName = "Smith",
        jobTitle = "Developer",
        email = "john.smith@test.com",
        phoneNumber = "12345678",
        secondaryPhoneNumber = "87654321",
        isTermsAndConditionsAccepted = true,
    };
}
