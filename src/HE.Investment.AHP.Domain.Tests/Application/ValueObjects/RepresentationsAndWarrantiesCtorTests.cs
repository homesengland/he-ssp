using FluentAssertions;
using HE.Investment.AHP.Domain.Application.Constants;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Application.ValueObjects;

public class RepresentationsAndWarrantiesCtorTests
{
    [Fact]
    public void ShouldCreateRepresentationsAndWarranties_WhenValueIsChecked()
    {
        // given && when
        var action = () => RepresentationsAndWarranties.FromString("checked");

        // then
        action.Should().NotThrow<DomainValidationException>();
        action().Value.Should().BeTrue();
    }

    [Theory]
    [InlineData("dd")]
    [InlineData("5")]
    [InlineData("almost checked")]
    public void ShouldThrowDomainValidationException_WhenStringIsNotChecked(string representationsAndWarranties)
    {
        // given && when
        var action = () => RepresentationsAndWarranties.FromString(representationsAndWarranties);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithMessage(ApplicationValidationErrors.MissingConfirmation);
    }
}
