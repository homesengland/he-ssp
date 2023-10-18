using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Contract.User.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.TestData;
public static class UserDetailsTestData
{
    public static readonly UserDetails UserDetailsOne = new(
        FirstName.FromString("John"),
        LastName.FromString("Doe"),
        JobTitle.FromString("Director"),
        "john.doe@test.com",
        TelephoneNumber.FromString("888888888"),
        TelephoneNumber.FromString(string.Empty, nameof(UserDetails.SecondaryTelephoneNumber)),
        true);
}
