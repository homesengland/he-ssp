using FluentAssertions;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.Section106Tests;

public class WithLocalAuthorityConfirmationTests
{
    [Fact]
    public void ShouldReturnLocalAuthorityConfirmation_WhenValueIsProvided()
    {
        // given
        var localAuthorityConfirmation = "localAuthorityConfirmation";

        // when
        var section106 = new Section106(true, localAuthorityConfirmation: localAuthorityConfirmation);

        // then
        section106.LocalAuthorityConfirmation.Should().Be(localAuthorityConfirmation);
    }

    [Fact]
    public void ShouldReturnNullLocalAuthorityConfirmation_WhenValueIsNotProvided()
    {
        // when
        var section106 = new Section106(true);

        // then
        section106.LocalAuthorityConfirmation.Should().BeNull();
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenLocalAuthorityConfirmationExceedsMaxLength()
    {
        // given
        var localAuthorityConfirmation = new string('a', 1501);

        // when
        var action = () => new Section106(true).WithLocalAuthorityConfirmation(localAuthorityConfirmation);

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == ValidationErrorMessage.StringLengthExceeded("local authority confirmation", MaximumInputLength.LongInput));
    }
}
