using HE.InvestmentLoans.BusinessLogic.Tests.User.TestData;
using HE.InvestmentLoans.BusinessLogic.User.Entities;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
public class UserDetailsEntityTestBuilder
{
    private readonly UserDetails _item;

    private UserDetailsEntityTestBuilder(UserDetails userDetails)
    {
        _item = userDetails;
    }

    public static UserDetailsEntityTestBuilder New() =>
        new(new UserDetails(
            UserDetailsTestData.UserDetailsOne.FirstName,
            UserDetailsTestData.UserDetailsOne.LastName,
            UserDetailsTestData.UserDetailsOne.JobTitle,
            UserDetailsTestData.UserDetailsOne.Email,
            UserDetailsTestData.UserDetailsOne.TelephoneNumber,
            UserDetailsTestData.UserDetailsOne.SecondaryTelephoneNumber,
            UserDetailsTestData.UserDetailsOne.IsTermsAndConditionsAccepted));

    public UserDetails Build()
    {
        return _item;
    }
}
