using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.BusinessLogic.User.Repositories;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;
using HE.InvestmentLoans.Contract.User.ValueObjects;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.UserRepositoryTests;
public class GetUserDetailsTests : TestBase<LoanUserRepository>
{
    [Fact]
    public async Task ShouldReturnUserDetailsEntity_WhenUserGlobalIdIsCorrect()
    {
        // given
        var contactDto = ContactServiceMockTestBuilder
            .New()
            .Register(this)
            .ContactDtoFromMock;

        ContactServiceMockTestBuilder
            .New()
            .ReturnContactDto(contactDto)
            .Register(this);

        // when
        var result = await TestCandidate.GetUserDetails(UserGlobalId.From(contactDto.contactId));

        // then
        result.FirstName.Should().Be(contactDto.firstName);
        result.Surname.Should().Be(contactDto.lastName);
        result.Email.Should().Be(contactDto.email);
        result.JobTitle.Should().Be(contactDto.jobTitle);
        result.TelephoneNumber.Should().Be(contactDto.phoneNumber);
        result.SecondaryTelephoneNumber.Should().Be(contactDto.secondaryPhoneNumber);
        result.IsTermsAndConditionsAccepted.Should().Be(contactDto.isTermsAndConditionsAccepted);
    }

    [Fact]
    public async Task ShouldThrowNotFoundException_WhenUserGlobalIdDoesNotExist()
    {
        // given
        var contactDto = ContactServiceMockTestBuilder
            .New()
            .Register(this)
            .ContactDtoFromMock;

        ContactServiceMockTestBuilder
            .New()
            .ReturnContactDto(contactDto)
            .Register(this);

        var wrongUserGlobalId = "Wrong user global id";

        // when
        var action = () => TestCandidate.GetUserDetails(UserGlobalId.From(wrongUserGlobalId));

        // then
        await action.Should().ThrowExactlyAsync<NotFoundException>().WithMessage($"*{wrongUserGlobalId}*");
    }
}
