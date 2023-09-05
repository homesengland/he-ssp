extern alias Org;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Organization.TestData;
public static class OrganizationDetailsDtoTestData
{
    public static readonly OrganizationDetailsDto OrganizationDetailsDto = new()
    {
        registeredCompanyName = "Test company",
        companyRegistrationNumber = "112233",
        city = "Warsaw",
        country = "Poland",
        postalcode = "123456",
        addressLine1 = "Aleje Jerozolimskie",
        addressLine2 = "100",
        addressLine3 = "12",
        compayAdminContactEmail = "admin@test.com",
        compayAdminContactName = "Joe Doe",
        compayAdminContactTelephone = "888888888",
        rpNumber = "10",
    };
}
