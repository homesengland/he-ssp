extern alias Org;

using HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;
using Org::HE.Investments.Organisation.LocalAuthorities.Mappers;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.Mappers.LocalAuthorityMapperTests;

public class MapToLocalAuthorityTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void ShouldReturnLocalAuthority_WhenLocalAuthorityDtoIsProvided(int localAuthorityDtoIndex)
    {
        // given
        var localAuthorityDto = LocalAuthorityDtoTestData.LocalAuthoritiesDtoList[localAuthorityDtoIndex];

        // when
        var result = LocalAuthorityMapper.MapToLocalAuthority(localAuthorityDto.onsCode, localAuthorityDto.name);

        // then
        result?.Id.ToString().Should().Be(localAuthorityDto.onsCode);
        result?.Name.Should().Be(localAuthorityDto.name);
    }
}
