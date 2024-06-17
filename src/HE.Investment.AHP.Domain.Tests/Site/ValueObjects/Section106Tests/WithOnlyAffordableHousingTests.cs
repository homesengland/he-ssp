using FluentAssertions;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.Section106Tests;

public class WithOnlyAffordableHousingTests
{
    [Fact]
    public void ShouldReturnOnlyAffordableHousing_WhenValueIsProvided()
    {
        // given
        var onlyAffordableHousing = true;

        // when
        var section106 = new Section106(true).WithOnlyAffordableHousing(onlyAffordableHousing);

        // then
        section106.OnlyAffordableHousing.Should().Be(onlyAffordableHousing);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenOnlyAffordableHousingIsNotProvided()
    {
        // given && when
        var action = () => new Section106(true).WithOnlyAffordableHousing(null);

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == ValidationErrorMessage.MustProvideRequiredField("100% Affordable Housing answer"));
    }

    [Fact]
    public void ShouldClearFollowUpAnswers_WhenOnlyAffordableHousingWasChanged()
    {
        // given
        var section = new Section106(true, true, false, true, false, "test");

        // when
        var result = section.WithOnlyAffordableHousing(true);

        // then
        result.GeneralAgreement.Should().BeTrue();
        result.AffordableHousing.Should().BeTrue();
        result.OnlyAffordableHousing.Should().BeTrue();
        result.AdditionalAffordableHousing.Should().BeNull();
        result.CapitalFundingEligibility.Should().BeNull();
        result.LocalAuthorityConfirmation.Should().BeNull();
    }
}
