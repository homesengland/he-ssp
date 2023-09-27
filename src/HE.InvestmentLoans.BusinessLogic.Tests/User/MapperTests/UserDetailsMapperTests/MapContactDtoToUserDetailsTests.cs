extern alias Org;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.BusinessLogic.User;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;
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
        userDetailsEntity.FirstName.Should().Be(contactDto.firstName);
        userDetailsEntity.LastName.Should().Be(contactDto.lastName);
        userDetailsEntity.JobTitle.Should().Be(contactDto.jobTitle);
        userDetailsEntity.Email.Should().Be(contactDto.email);
        userDetailsEntity.TelephoneNumber.Should().Be(contactDto.phoneNumber);
        userDetailsEntity.SecondaryTelephoneNumber.Should().Be(contactDto.secondaryPhoneNumber);
        userDetailsEntity.IsTermsAndConditionsAccepted.Should().Be(contactDto.isTermsAndConditionsAccepted);
    }
}
