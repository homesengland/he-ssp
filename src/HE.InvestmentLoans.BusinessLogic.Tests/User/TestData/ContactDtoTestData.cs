extern alias Org;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
