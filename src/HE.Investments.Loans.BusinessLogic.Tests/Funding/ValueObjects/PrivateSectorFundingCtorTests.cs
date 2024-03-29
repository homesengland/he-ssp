using HE.Investments.Common.Contract.Constants;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.BusinessLogic.Tests.Assertions;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.Contract.Funding.ValueObjects;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Funding.ValueObjects;
public class PrivateSectorFundingCtorTests
{
    [Fact]
    public void ShouldCreatePrivateSectorFundingWithApplyResult()
    {
        // given
        var response = CommonResponse.Yes;
        var applyResult = "apply result";

        // when
        var action = () => PrivateSectorFunding.FromString(response, applyResult, null);

        // then
        action.Should().NotThrow<DomainValidationException>();
        action().IsApplied.Should().BeTrue();
        action().PrivateSectorFundingApplyResult.Should().Be(applyResult);
        action().PrivateSectorFundingNotApplyingReason.Should().BeNull();
    }

    [Fact]
    public void ShouldCreatePrivateSectorFundingWithNotApplyingApplyReason()
    {
        // given
        var response = CommonResponse.No;
        var notApplyingReason = "not applying reason";

        // when
        var action = () => PrivateSectorFunding.FromString(response, null, notApplyingReason);

        // then
        action.Should().NotThrow<DomainValidationException>();
        action().IsApplied.Should().BeFalse();
        action().PrivateSectorFundingApplyResult.Should().BeNull();
        action().PrivateSectorFundingNotApplyingReason.Should().Be(notApplyingReason);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenPrivateSectorFundingIsTrueButNoApplyResultIsProvided()
    {
        // given
        var response = true;
        string? applyResult = null;

        // when
        var action = () => PrivateSectorFunding.New(response, applyResult, null);

        // then
        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.EnterMoreDetails);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenPrivateSectorFundingIsFalseButNoNotApplyingReasonIsProvided()
    {
        // given
        var response = false;
        string? notApplyingReason = null;

        // when
        var action = () => PrivateSectorFunding.New(response, null, notApplyingReason);

        // then
        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.EnterMoreDetails);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenApplyResultInputIsLongerThan1500Chars()
    {
        // given
        var response = true;
        var applyResult = new string('*', 1501);

        // when
        var action = () => PrivateSectorFunding.New(response, applyResult, null);

        // then
        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.PrivateSectorFundingResult));
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenNotApplyingReasonInputIsLongerThan1500Chars()
    {
        // given
        var response = false;
        var notApplyingReason = new string('*', 1501);

        // when
        var action = () => PrivateSectorFunding.New(response, null, notApplyingReason);

        // then
        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.PrivateSectorFundingReason));
    }
}
