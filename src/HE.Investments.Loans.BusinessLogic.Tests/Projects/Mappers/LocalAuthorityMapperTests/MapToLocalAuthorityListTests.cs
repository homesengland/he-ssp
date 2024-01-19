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
        result[0].Id.ToString().Should().Be(localAuthoritiesDto[0].onsCode);
        result[0].Name.Should().Be(localAuthoritiesDto[0].name);
        result[result.Count - 1].Name.Should().Be(localAuthoritiesDto[result.Count - 1].name);
        result[result.Count - 1].Name.Should().Be(localAuthoritiesDto[result.Count - 1].name);
        result.Count.Should().Be(localAuthoritiesDto.Count);
    }
}
