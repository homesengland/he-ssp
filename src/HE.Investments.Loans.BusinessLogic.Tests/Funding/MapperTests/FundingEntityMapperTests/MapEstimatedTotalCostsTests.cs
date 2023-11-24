using HE.Investments.Loans.BusinessLogic.Funding.Mappers;
using HE.Investments.Loans.Contract.Funding.ValueObjects;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Funding.MapperTests.FundingEntityMapperTests;
public class MapEstimatedTotalCostsTests
{
    [Fact]
    public void ShouldReturnFilledEstimatedTotalCosts_WhenDecimalIsProvided()
    {
        // given
        var newDecimalNumber = 2.45m;

        // when
        var estimatedTotalCosts = FundingEntityMapper.MapEstimatedTotalCosts(newDecimalNumber);

        // then
        estimatedTotalCosts!.Value.Should().Be(newDecimalNumber);
    }

    [Fact]
    public void ShouldReturnDecimal_WhenEstimatedTotalCostsIsProvided()
    {
        // given
        var estimatedTotalCosts = EstimatedTotalCosts.New(5.5m);

        // when
        var estimatedTotalCostsDecimal = FundingEntityMapper.MapEstimatedTotalCosts(estimatedTotalCosts);

        // then
        estimatedTotalCostsDecimal!.Should().Be(estimatedTotalCosts.Value);
    }

    [Fact]
    public void ShouldReturnNull_WhenProvidedDataAreMissing()
    {
        // given
        decimal? nullValue = null;

        // when
        var estimatedTotalCosts = FundingEntityMapper.MapEstimatedTotalCosts(nullValue);

        // then
        estimatedTotalCosts.Should().BeNull();
    }
}
