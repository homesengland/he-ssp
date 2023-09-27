using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Contract.Application.Enums;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.UserEntityTests.UserDetailsEntityTests;
public class ProvideUserDetailsTests
{
    [Fact]
    public void ShouldChangeUserDetails_WhenProvideCorrectData()
    {
        // given
        var userDetailsEntity = UserDetailsEntityTestBuilder.New().Build();

        var newUserDetails = new UserDetails("Jacob", "Smith", "Developer", "john.smith@test.com", "12345678", "87654321", false);

        // when
        userDetailsEntity.ProvideUserDetails(
            newUserDetails.FirstName!,
            newUserDetails.LastName!,
            newUserDetails.JobTitle!,
            newUserDetails.TelephoneNumber!,
            newUserDetails.SecondaryTelephoneNumber!,
            newUserDetails.Email!);

        // then
        userDetailsEntity.FirstName.Should().Be(newUserDetails.FirstName);
        userDetailsEntity.LastName.Should().Be(newUserDetails.LastName);
        userDetailsEntity.JobTitle.Should().Be(newUserDetails.JobTitle);
        userDetailsEntity.TelephoneNumber.Should().Be(newUserDetails.TelephoneNumber);
        userDetailsEntity.SecondaryTelephoneNumber.Should().Be(newUserDetails.SecondaryTelephoneNumber);
        userDetailsEntity.Email.Should().Be(newUserDetails.Email);
    }
}
