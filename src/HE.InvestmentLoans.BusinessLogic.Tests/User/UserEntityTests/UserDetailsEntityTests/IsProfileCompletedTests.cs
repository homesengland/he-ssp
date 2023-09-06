using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.UserEntityTests.UserDetailsEntityTests;
public class IsProfileCompletedTests
{
    [Fact]
    public void ShouldReturnTrue_WhenAllUserDetailsAreProvided()
    {
        // given
        var userDetailsEntity = UserDetailsEntityTestBuilder.New().Build();

        // when
        var result = userDetailsEntity.IsProfileCompleted();

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalse_WhenJobTitleNameAreProvided()
    {
        // given
        var userDetails = new UserDetails("John", "Smith", null, "john.smith@test.com", "12345678", "87654321", false);

        // when
        var result = userDetails.IsProfileCompleted();

        // then
        result.Should().BeFalse();
    }
}
