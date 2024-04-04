using FluentAssertions;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.Contract.Common;
using Xunit;

namespace HE.Investments.Loans.Contract.Tests.Common;

public class LongTextTests
{
    [Fact]
    public void ShouldCreateLongText_WhenValueIsProvided()
    {
        // given
        var value = "This is a long text value, but not too long.";

        // when
        var action = () => new LongText(value, nameof(LongText), "long text value");

        // then
        action.Should().NotThrow<DomainValidationException>();
        action().Value.Should().Be(value);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenNoValueIsProvided()
    {
        // given
        var value = string.Empty;

        // when
        var action = () => new LongText(value, nameof(LongText), "long text value");

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == ValidationErrorMessage.MissingRequiredField("long text value"));
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenValueIsTooLong()
    {
        // given
        var value = new string('a', MaximumInputLength.LongInput + 1);

        // when
        var action = () => new LongText(value, nameof(LongText), "long text value");

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == ValidationErrorMessage.LongInputLengthExceeded("long text value"));
    }
}
