using FluentAssertions;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.Section106Tests;

public class WithAffordableHousingTests
{
    [Fact]
    public void ShouldReturnAffordableHousing_WhenValueIsProvided()
    {
        // given
        var affordableHousing = true;

        // when
        var section106 = new Section106(true).WithAffordableHousing(affordableHousing);

        // then
        section106.AffordableHousing.Should().Be(affordableHousing);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenAffordableHousingIsNotProvided()
    {
        // given && when
        var action = () => new Section106(true).WithAffordableHousing(null);

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == ValidationErrorMessage.MustProvideRequiredField("Affordable Housing answer"));
    }

    [Fact]
    public void ShouldClearFollowUpAnswers_WhenAffordableHousingWasChanged()
    {
        // given
        var section = new Section106(true, true, false, true, false, "test");

        // when
        var result = section.WithAffordableHousing(false);

        // then
        result.GeneralAgreement.Should().BeTrue();
        result.AffordableHousing.Should().BeFalse();
        result.OnlyAffordableHousing.Should().BeNull();
        result.AdditionalAffordableHousing.Should().BeNull();
        result.CapitalFundingEligibility.Should().BeNull();
        result.LocalAuthorityConfirmation.Should().BeNull();
    }
}
