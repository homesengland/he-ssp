using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure;
using HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.BusinessLogic.User.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using HE.InvestmentLoans.Contract.User.Commands;
using Moq;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.CommandHandlers;
public class ProvideUserDetailsCommandHandlerTests : TestBase<ProvideUserDetailsCommandHandler>
{
    [Fact]
    public async Task ShouldUpdateUserDetails_WhenProvidedCorrectData()
    {
        // given
        var userAccount = LoanUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var userDetails = LoanUserContextTestBuilder
            .New()
            .Register(this)
            .UserDetailsFromMock;

        var userRepositoryMock = UserRepositoryTestBuilder
            .New()
            .ReturnUserDetailsEntity(userAccount.UserGlobalId, userDetails)
            .BuildMockAndRegister(this);

        var newUserDetails = new UserDetails("Jacob", "Smith", "Developer", "john.smith@test.com", "12345678", "87654321", false);

        // when
        await TestCandidate.Handle(
            new ProvideUserDetailsCommand(
                newUserDetails.FirstName!,
                newUserDetails.Surname!,
                newUserDetails.JobTitle!,
                newUserDetails.TelephoneNumber!,
                newUserDetails.SecondaryTelephoneNumber!),
            CancellationToken.None);

        // then
        userDetails.FirstName.Should().Be(newUserDetails.FirstName);
        userDetails.Surname.Should().Be(newUserDetails.Surname);
        userDetails.JobTitle.Should().Be(newUserDetails.JobTitle);
        userDetails.TelephoneNumber.Should().Be(newUserDetails.TelephoneNumber);
        userDetails.SecondaryTelephoneNumber.Should().Be(newUserDetails.SecondaryTelephoneNumber);
        userRepositoryMock.Verify(x =>
            x.SaveAsync(It.Is<UserDetails>(y => y.FirstName! == newUserDetails.FirstName), userAccount.UserGlobalId, CancellationToken.None));
        userRepositoryMock.Verify(x =>
            x.SaveAsync(It.Is<UserDetails>(y => y.Surname! == newUserDetails.Surname), userAccount.UserGlobalId, CancellationToken.None));
        userRepositoryMock.Verify(x =>
            x.SaveAsync(It.Is<UserDetails>(y => y.JobTitle! == newUserDetails.JobTitle), userAccount.UserGlobalId, CancellationToken.None));
        userRepositoryMock.Verify(x =>
            x.SaveAsync(It.Is<UserDetails>(y => y.TelephoneNumber! == newUserDetails.TelephoneNumber), userAccount.UserGlobalId, CancellationToken.None));
        userRepositoryMock.Verify(x =>
            x.SaveAsync(It.Is<UserDetails>(y => y.SecondaryTelephoneNumber! == newUserDetails.SecondaryTelephoneNumber), userAccount.UserGlobalId, CancellationToken.None));
    }
}
