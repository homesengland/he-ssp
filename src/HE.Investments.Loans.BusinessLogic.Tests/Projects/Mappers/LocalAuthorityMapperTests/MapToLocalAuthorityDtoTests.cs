using HE.Investments.Loans.BusinessLogic.Projects.Repositories.Mappers;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.Mappers.LocalAuthorityMapperTests;

public class MapToLocalAuthorityDtoTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void ShouldReturnLocalAuthorityDto_WhenLocalAuthorityEntityIsProvided(int localAuthorityIndex)
    {
        // given
        var localAuthority = LocalAuthorityTestData.LocalAuthoritiesList[localAuthorityIndex];

        // when
        var result = LocalAuthorityMapper.MapToLocalAuthorityDto(localAuthority);

        // then
        result?.onsCode.Should().Be(localAuthority.Id.ToString());
        result?.name.Should().Be(localAuthority.Name);
    }
}
