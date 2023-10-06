using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Contract.User.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.TestData;
public static class UserDetailsTestData
{
    public static readonly UserDetails UserDetailsOne = new(
        FirstName.New("John"),
        LastName.New("Doe"),
        JobTitle.New("Director"),
        "john.doe@test.com",
        TelephoneNumber.New("888888888"),
        SecondaryTelephoneNumber.New(string.Empty),
        true);
}
