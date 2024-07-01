using FluentAssertions;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.Section106Tests;

public class WithGeneralAgreementTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldReturnGeneralAgreementEligibility_WhenGeneralAgreementIsProvided(bool value)
    {
        // given & when
        var section106 = new Section106().WithGeneralAgreement(value);

        // then
        section106.GeneralAgreement.Should().Be(value);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenGeneralAgreementIsNotProvided()
    {
        // given
        var section = new Section106(true);

        // when
        var action = () => section.WithGeneralAgreement(null);

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == ValidationErrorMessage.MustBeSelectedYes("there is a s106 agreement in place or in discussion"));
    }

    [Fact]
    public void ShouldClearFollowUpAnswers_WhenGeneralAgreementWasChanged()
    {
        // given
        var section = new Section106(true, true, false, true, false, "test");

        // when
        var result = section.WithGeneralAgreement(false);

        // then
        result.GeneralAgreement.Should().BeFalse();
        result.AffordableHousing.Should().BeNull();
        result.OnlyAffordableHousing.Should().BeNull();
        result.AdditionalAffordableHousing.Should().BeNull();
        result.CapitalFundingEligibility.Should().BeNull();
        result.LocalAuthorityConfirmation.Should().BeNull();
    }
}
