extern alias Org;

using Org::HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.TestData;

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
