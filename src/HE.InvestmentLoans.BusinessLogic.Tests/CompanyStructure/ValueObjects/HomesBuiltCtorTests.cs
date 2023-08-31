using System.Globalization;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure;
using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Constants;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.ValueObjects;

public class HomesBuiltCtorTests
{
    [Theory]
    [InlineData("0")]
    [InlineData("1")]
    [InlineData("99999")]
    public void ShouldCreateHomesBuilt(string homesBuildAsString)
    {
        // given & when
        var action = () => HomesBuilt.FromString(homesBuildAsString);

        // then
        action.Should().NotThrow<DomainValidationException>();
        action().Value.Should().Be(int.Parse(homesBuildAsString, CultureInfo.InvariantCulture));
    }

    [Theory]
    [InlineData("notNumber")]
    [InlineData("  ")]
    public void ShouldThrowDomainValidationException_WhenStringIsNotNumber(string homesBuildAsString)
    {
        // given & when
        var action = () => HomesBuilt.FromString(homesBuildAsString);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.HomesBuiltIncorretInput);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenStringIsDecimal()
    {
        // given
        var homesBuildAsString = 11.1m.ToString(CultureInfo.InvariantCulture);

        // when
        var action = () => HomesBuilt.FromString(homesBuildAsString);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.HomesBuiltDecimalNumber);
    }

    [Theory]
    [InlineData("-1")]
    [InlineData("100000")]
    public void ShouldThrownDomainValidationException_WhenStringIsNumberOutOfAllowedRange(string homesBuildAsString)
    {
        // given & when
        var action = () => HomesBuilt.FromString(homesBuildAsString);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.HomesBuiltIncorrectNumber);
    }
}
