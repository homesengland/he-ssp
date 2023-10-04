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
        contactDto.firstName.Should().Be(userDetailsEntity.FirstName);
        contactDto.lastName.Should().Be(userDetailsEntity.LastName);
        contactDto.jobTitle.Should().Be(userDetailsEntity.JobTitle);
        contactDto.email.Should().Be(userDetailsEntity.Email);
        contactDto.phoneNumber.Should().Be(userDetailsEntity.TelephoneNumber);
        contactDto.secondaryPhoneNumber.Should().Be(userDetailsEntity.SecondaryTelephoneNumber);
        contactDto.isTermsAndConditionsAccepted.Should().Be(userDetailsEntity.IsTermsAndConditionsAccepted);
    }
}
