using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure;
using HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestData;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;

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
            UserDetailsTestData.UserDetailsOne.Surname,
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
