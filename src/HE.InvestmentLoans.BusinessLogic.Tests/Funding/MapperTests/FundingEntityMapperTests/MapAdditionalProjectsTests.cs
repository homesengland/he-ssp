using HE.InvestmentLoans.BusinessLogic.Funding.Mappers;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Funding.MapperTests.FundingEntityMapperTests;
public class MapAdditionalProjectsTests
{
    [Fact]
    public void ShouldReturnFilledAdditionalProjects_WhenDataAreFilled()
    {
        // given
        var isThereAnyAdditionalProject = true;

        // when
        var additionalProjects = FundingEntityMapper.MapAdditionalProjects(isThereAnyAdditionalProject);

        // then
        additionalProjects!.IsThereAnyAdditionalProject.Should().Be(isThereAnyAdditionalProject);
    }

    [Fact]
    public void ShouldReturnNull_WhenProvidedDataAreMissing()
    {
        // given
        bool? isThereAnyAdditionalProject = null;

        // when
        var additionalProjects = FundingEntityMapper.MapAdditionalProjects(isThereAnyAdditionalProject);

        // then
        additionalProjects.Should().BeNull();
    }
}
