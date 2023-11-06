using HE.InvestmentLoans.BusinessLogic.Organization.Entities;
using HE.InvestmentLoans.BusinessLogic.Organization.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Organization.TestData;

public static class OrganisationEntityTestData
{
    public static readonly OrganisationEntity OrganisationEntity =
        new(
            new OrganisationName("PwC Poland"),
            new OrganisationAddress("United Street", "100", null, "London", "CH65 1AY", null, "England"),
            new OrganisationPhoneNumber("666 222 444"));
}
