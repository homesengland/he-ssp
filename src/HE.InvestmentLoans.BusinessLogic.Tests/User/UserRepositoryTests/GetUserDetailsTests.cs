using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.BusinessLogic.User.Repositories;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Contract.User.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;
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
        result.FirstName.Should().Be(FirstName.FromString(contactDto.firstName));
        result.LastName.Should().Be(LastName.FromString(contactDto.lastName));
        result.Email.Should().Be(contactDto.email);
        result.JobTitle.Should().Be(JobTitle.FromString(contactDto.jobTitle));
        result.TelephoneNumber.Should().Be(TelephoneNumber.FromString(contactDto.phoneNumber));
        result.SecondaryTelephoneNumber.Should().Be(TelephoneNumber.FromString(contactDto.secondaryPhoneNumber));
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
