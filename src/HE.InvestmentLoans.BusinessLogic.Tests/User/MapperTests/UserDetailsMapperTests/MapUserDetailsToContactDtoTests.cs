using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.BusinessLogic.User;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.MapperTests.UserDetailsMapperTests;
public class MapUserDetailsToContactDtoTests
{
    [Fact]
    public void ShouldReturnFilledContactDto_WhenMappingDataFromUserDetails()
    {
        // given
        var userDetailsEntity = UserDetailsEntityTestBuilder.New().Build();

        // when
        var contactDto = UserDetailsMapper.MapUserDetailsToContactDto(userDetailsEntity);

        // then
        contactDto.firstName.Should().Be(userDetailsEntity.FirstName?.ToString());
        contactDto.lastName.Should().Be(userDetailsEntity.LastName?.ToString());
        contactDto.jobTitle.Should().Be(userDetailsEntity.JobTitle?.ToString());
        contactDto.email.Should().Be(userDetailsEntity.Email);
        contactDto.phoneNumber.Should().Be(userDetailsEntity.TelephoneNumber?.ToString());
        contactDto.secondaryPhoneNumber.Should().Be(userDetailsEntity.SecondaryTelephoneNumber?.ToString());
        contactDto.isTermsAndConditionsAccepted.Should().Be(userDetailsEntity.IsTermsAndConditionsAccepted);
    }
}
