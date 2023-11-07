extern alias Org;

using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.Account.Domain.Tests.Organisation.TestData;

public static class OrganizationDetailsDtoTestData
{
    public static readonly OrganizationDetailsDto OrganizationDetailsDto = new()
    {
        registeredCompanyName = "Test company",
        companyRegistrationNumber = "112233",
        city = "Warsaw",
        country = "Poland",
        postalcode = "CH65 1AY",
        addressLine1 = "Aleje Jerozolimskie",
        addressLine2 = "100",
        addressLine3 = "12",
        compayAdminContactEmail = "admin@test.com",
        compayAdminContactName = "Joe Doe",
        compayAdminContactTelephone = "888888888",
        rpNumber = "10",
        organisationPhoneNumber = "543 123 864",
    };
}
