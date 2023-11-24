using HE.Investments.Loans.BusinessLogic.Projects.Repositories.Mappers;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.Mappers.LocalAuthorityMapperTests;

public class MapToLocalAuthorityListTests
{
    [Fact]
    public void ShouldReturnLocalAuthoritiesList_WhenListOfLocalAuthorityDtoIsProvided()
    {
        // given
        var localAuthoritiesDto = LocalAuthorityDtoTestData.LocalAuthoritiesDtoList;

        // when
        var result = LocalAuthorityMapper.MapToLocalAuthorityList(localAuthoritiesDto);

        // then
        result.First().Id.ToString().Should().Be(localAuthoritiesDto.First().onsCode);
        result.First().Name.Should().Be(localAuthoritiesDto.First().name);
        result.Last().Name.Should().Be(localAuthoritiesDto.Last().name);
        result.Last().Name.Should().Be(localAuthoritiesDto.Last().name);
        result.Count.Should().Be(localAuthoritiesDto.Count);
    }
}
