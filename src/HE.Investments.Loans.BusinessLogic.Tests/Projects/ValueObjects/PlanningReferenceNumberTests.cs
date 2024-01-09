using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.BusinessLogic.Tests.Assertions;
using HE.Investments.Loans.Common.Tests.TestData;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.ValueObjects;
public class PlanningReferenceNumberTests
{
    [Fact]
    public void CreateNumber_WhenExistsAndItsValueIsNotProvided()
    {
        var referenceNumber = new PlanningReferenceNumber(true, null!);

        referenceNumber.Exists.Should().BeTrue();
    }

    [Fact]
    public void CreateNumber_WhenExistsAndItsValueIsProvided()
    {
        var referenceNumber = PlanningReferenceNumber.FromString(CommonResponse.Yes, "number");

        referenceNumber.Exists.Should().BeTrue();
        referenceNumber.Value.Should().Be("number");
    }

    [Fact]
    public void DoNotSetValue_WhenNumberDoesNotExist()
    {
        var referenceNumber = PlanningReferenceNumber.FromString(CommonResponse.No, "number");

        referenceNumber.Exists.Should().BeFalse();
        referenceNumber.Value.Should().BeNull();
    }

    [Fact]
    public void ShouldThrowValidationError_WhenNumberExceedsShortInputLimit()
    {
        var action = () => PlanningReferenceNumber.FromString(CommonResponse.Yes, TextTestData.TextThatExceedsShortInputLimit);

        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.PlanningReferenceNumber));
    }

    [Fact]
    public void ShouldNotThrowValidationError_WhenNumberExceedsShortInputLimitButNumberDoesNotExist()
    {
        var action = () => PlanningReferenceNumber.FromString(CommonResponse.No, TextTestData.TextThatExceedsShortInputLimit);

        action.Should().NotThrow<DomainValidationException>();
    }
}
