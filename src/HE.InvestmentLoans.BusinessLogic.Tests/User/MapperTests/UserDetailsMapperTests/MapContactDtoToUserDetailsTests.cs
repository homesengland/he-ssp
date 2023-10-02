extern alias Org;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.User.ValueObjects;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.MapperTests.UserDetailsMapperTests;
public class MapContactDtoToUserDetailsTests
{
    [Fact]
    public void ShouldReturnFilledUserDetails_WhenMappingDataFromContactDto()
    {
        // given
        var contactDto = ContactDtoTestBuilder.New().Build();

        // when
        var userDetailsEntity = UserDetailsMapper.MapContactDtoToUserDetails(contactDto);

        // then
        userDetailsEntity.FirstName.Should().Be(FirstName.FromString(contactDto.firstName));
        userDetailsEntity.LastName.Should().Be(LastName.FromString(contactDto.lastName));
        userDetailsEntity.JobTitle.Should().Be(JobTitle.FromString(contactDto.jobTitle));
        userDetailsEntity.Email.Should().Be(contactDto.email);
        userDetailsEntity.TelephoneNumber.Should().Be(TelephoneNumber.FromString(contactDto.phoneNumber));
        userDetailsEntity.SecondaryTelephoneNumber.Should().Be(SecondaryTelephoneNumber.FromString(contactDto.secondaryPhoneNumber));
        userDetailsEntity.IsTermsAndConditionsAccepted.Should().Be(contactDto.isTermsAndConditionsAccepted);
    }
}
