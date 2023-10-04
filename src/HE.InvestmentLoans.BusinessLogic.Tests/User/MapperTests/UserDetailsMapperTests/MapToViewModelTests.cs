using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.BusinessLogic.User;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.MapperTests.UserDetailsMapperTests;
public class MapToViewModelTests
{
    [Fact]
    public void ShouldReturnFilledUserDetailsViewModel_WhenMappingDataFromUserDetails()
    {
        // given
        var userDetailsEntity = UserDetailsEntityTestBuilder.New().Build();

        // when
        var userDetailsViewModel = UserDetailsMapper.MapToViewModel(userDetailsEntity);

        // then
        userDetailsViewModel.FirstName.Should().Be(userDetailsEntity.FirstName);
        userDetailsViewModel.LastName.Should().Be(userDetailsEntity.LastName);
        userDetailsViewModel.JobTitle.Should().Be(userDetailsEntity.JobTitle);
        userDetailsViewModel.TelephoneNumber.Should().Be(userDetailsEntity.TelephoneNumber);
        userDetailsViewModel.SecondaryTelephoneNumber.Should().Be(userDetailsEntity.SecondaryTelephoneNumber);
    }
}
