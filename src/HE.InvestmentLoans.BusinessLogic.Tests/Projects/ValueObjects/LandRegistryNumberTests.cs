using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Messages;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.ValueObjects;
public class LandRegistryNumberTests
{
    [Fact]
    public void CreateNumber_WhenExistsAndItsValueIsProvided()
    {
        var referenceNumber = new LandRegistryTitleNumber("number");

        referenceNumber.Value.Should().Be("number");
    }

    [Fact]
    public void ShouldThrowValidationError_WhenNumberExceedsLongInputLimit()
    {
        var additionalInformation = new string('*', 1501);
        var action = () => new LandRegistryTitleNumber(additionalInformation);

        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.LocationLandRegistry));
    }

    [Fact]
    public void ShouldThrowValidationError_WhenNumberIsEmpty()
    {
        var action = () => new LandRegistryTitleNumber(string.Empty);

        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.EnterLandRegistryTitleNumber);
    }
}
