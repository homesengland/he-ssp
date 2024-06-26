using FluentAssertions;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.Section106Tests;

public class WithCapitalFundingEligibilityTests
{
    [Fact]
    public void ShouldReturnCapitalFundingEligibility_WhenValueIsProvided()
    {
        // given
        var capitalFundingEligibility = true;

        // when
        var section106 = new Section106(true).WithCapitalFundingEligibility(capitalFundingEligibility);

        // then
        section106.CapitalFundingEligibility.Should().Be(capitalFundingEligibility);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenCapitalFundingEligibilityIsNotProvided()
    {
        // given && when
        var action = () => new Section106(true).WithCapitalFundingEligibility(null);

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == ValidationErrorMessage.MustProvideRequiredField("Capital funding Eligibility answer"));
    }

    [Fact]
    public void ShouldClearFollowUpAnswers_WhenCapitalFundingEligibilityWasChanged()
    {
        // given
        var section = new Section106(true, true, false, true, false, "test");

        // when
        var result = section.WithCapitalFundingEligibility(true);

        // then
        result.GeneralAgreement.Should().BeTrue();
        result.AffordableHousing.Should().BeTrue();
        result.OnlyAffordableHousing.Should().BeFalse();
        result.AdditionalAffordableHousing.Should().BeTrue();
        result.CapitalFundingEligibility.Should().BeTrue();
        result.LocalAuthorityConfirmation.Should().BeNull();
    }
}
