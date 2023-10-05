using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.ValueObjects;
public class CoordinatesTests
{
    [Fact]
    public void CreateCoordinates_WhenValueIsCorrect()
    {
        var referenceNumber = new Coordinates("correct value");

        referenceNumber.Value.Should().Be("correct value");
    }

    [Fact]
    public void ShouldThrowValidationError_WhenCoordinatesExceedsLongInputLimit()
    {
        var additionalInformation = new string('*', 1501);
        var action = () => new Coordinates(additionalInformation);

        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.LocationCoordinates));
    }

    [Fact]
    public void ShouldThrowValidationError_WhenNumberIsEmpty()
    {
        var action = () => new Coordinates(string.Empty);

        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.EnterCoordinates);
    }
}
