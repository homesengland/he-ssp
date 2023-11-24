using HE.Investments.Account.Shared.User.Entities;
using HE.Investments.Account.Shared.User.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.Tests.User.TestData;
public static class UserDetailsTestData
{
    public static readonly UserProfileDetails UserDetailsOne = new(
        new FirstName("John"),
        new LastName("Doe"),
        new JobTitle("Director"),
        "john.doe@test.com",
        new TelephoneNumber("888888888"),
        null,
        true);
}
