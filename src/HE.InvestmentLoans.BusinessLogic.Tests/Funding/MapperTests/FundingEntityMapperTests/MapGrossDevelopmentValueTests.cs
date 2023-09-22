using HE.InvestmentLoans.BusinessLogic.Funding.Mappers;
using HE.InvestmentLoans.Contract.Funding.ValueObjects;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Funding.MapperTests.FundingEntityMapperTests;
public class MapGrossDevelopmentValueTests
{
    [Fact]
    public void ShouldReturnFilledGrossDevelopmentValue_WhenDecimalIsProvided()
    {
        // given
        var newDecimalNumber = 2.45m;

        // when
        var grossDevelopmentValue = FundingEntityMapper.MapGrossDevelopmentValue(newDecimalNumber);

        // then
        grossDevelopmentValue!.Value.Should().Be(newDecimalNumber);
    }

    [Fact]
    public void ShouldReturnDecimal_WhenGrossDevelopmentValueIsProvided()
    {
        // given
        var grossDevelopmentValue = GrossDevelopmentValue.New(5.5m);

        // when
        var grossDevelopmentValueDecimal = FundingEntityMapper.MapGrossDevelopmentValue(grossDevelopmentValue);

        // then
        grossDevelopmentValueDecimal!.Should().Be(grossDevelopmentValue.Value);
    }

    [Fact]
    public void ShouldReturnNull_WhenProvidedDataAreMissing()
    {
        // given
        decimal? nullValue = null;

        // when
        var grossDevelopmentValue = FundingEntityMapper.MapGrossDevelopmentValue(nullValue);

        // then
        grossDevelopmentValue.Should().BeNull();
    }
}
