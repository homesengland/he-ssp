using System.Globalization;
using FluentAssertions;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure;
using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Constants;

namespace HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure;

[TestClass]
public class HomesBuiltCtorTests
{
    [TestMethod]
    [DataRow("0")]
    [DataRow("1")]
    [DataRow("99999")]
    public void HomesBuiltShouldBeCreated(string homesBuildAsString)
    {
        // given & when
        var action = () => HomesBuilt.FromString(homesBuildAsString);

        // then
        action.Should().NotThrow<DomainValidationException>();
        action().Value.Should().Be(int.Parse(homesBuildAsString, CultureInfo.InvariantCulture));
    }

    [TestMethod]
    [DataRow("notNumber")]
    [DataRow("  ")]
    public void DomainValidationExceptionShouldBeThrown_WhenStringIsNotNumber(string homesBuildAsString)
    {
        // given & when
        var action = () => HomesBuilt.FromString(homesBuildAsString);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.HomesBuiltIncorretInput);
    }

    [TestMethod]
    public void DomainValidationExceptionShouldBeThrown_WhenStringIsDecimal()
    {
        // given
        var homesBuildAsString = 11.1m.ToString(CultureInfo.InvariantCulture);

        // when
        var action = () => HomesBuilt.FromString(homesBuildAsString);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.HomesBuiltDecimalNumber);
    }

    [TestMethod]
    [DataRow("-1")]
    [DataRow("100000")]
    public void DomainValidationExceptionShouldBeThrown_WhenStringIsNumberOutOfAllowedRange(string homesBuildAsString)
    {
        // given & when
        var action = () => HomesBuilt.FromString(homesBuildAsString);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.HomesBuiltIncorrectNumber);
    }
}
