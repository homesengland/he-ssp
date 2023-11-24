using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.BusinessLogic.Tests.Assertions;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.ValueObjects;
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
