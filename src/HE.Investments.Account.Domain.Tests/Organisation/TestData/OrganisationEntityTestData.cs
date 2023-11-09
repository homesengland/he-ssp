using HE.Investments.Account.Domain.Organisation.Entities;
using HE.Investments.Account.Domain.Organisation.ValueObjects;

namespace HE.Investments.Account.Domain.Tests.Organisation.TestData;

public static class OrganisationEntityTestData
{
    public static readonly OrganisationEntity OrganisationEntity =
        new(
            new OrganisationName("PwC Poland"),
            new OrganisationAddress("United Street", "100", null, "London", "CH65 1AY", null, "England"),
            new OrganisationPhoneNumber("666 222 444"));
}
