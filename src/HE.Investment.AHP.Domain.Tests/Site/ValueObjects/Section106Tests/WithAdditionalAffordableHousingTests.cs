using FluentAssertions;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.Section106Tests;

public class WithAdditionalAffordableHousingTests
{
    [Fact]
    public void ShouldReturnAdditionalAffordableHousing_WhenValueIsProvided()
    {
        // given
        var additionalAffordableHousing = true;

        // when
        var section106 = new Section106(true).WithAdditionalAffordableHousing(additionalAffordableHousing);

        // then
        section106.AdditionalAffordableHousing.Should().Be(additionalAffordableHousing);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenAdditionalAffordableHousingIsNotProvided()
    {
        // given && when
        var action = () => new Section106(true).WithAdditionalAffordableHousing(null);

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == ValidationErrorMessage.MustProvideRequiredField("Additional Affordable Housing answer"));
    }
}
