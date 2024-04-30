using HE.Investments.Account.Api.Contract.Organisation;
using HE.Investments.Account.Api.Contract.User;

namespace HE.Investments.TestsUtils.TestData;

public static class AccountTestData
{
    public static ProfileDetails PaulSmith(string emailAddress)
        => new("Paul", "Smith", "Developer", emailAddress, "020 960 194", null, true);

    public static UserOrganisation PwCAdmin(string organisationId) =>
        new(new OrganisationDetails(organisationId, "PwC", "100200300", "Warsaw", false), [UserRole.Admin]);
}
