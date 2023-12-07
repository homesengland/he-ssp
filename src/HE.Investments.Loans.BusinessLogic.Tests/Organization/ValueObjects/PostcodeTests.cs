using HE.Investments.Account.Domain.Organisation.ValueObjects;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.BusinessLogic.Tests.Assertions;
using HE.Investments.Loans.Common.Utils.Constants;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Organization.ValueObjects;

public class PostcodeTests
{
    [Theory]
    [InlineData("PO16 7GZ")]
    [InlineData("po16 7gz")]
    public void ShouldCreate(string value)
    {
        // given & when
        var result = new Postcode(value);

        // then
        result.Value.Should().Be(value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldThrowException_WhenMissingOrEmpty(string value)
    {
        // given & when
        var result = () => new Postcode(value);

        // then
        result.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(OrganisationErrorMessages.MissingOrganisationPostCode);
    }

    [Fact]
    public void ShouldThrowException_WhenTooLong()
    {
        // given
        var value = string.Join(string.Empty, Enumerable.Repeat(0, 101).Select(n => (char)new Random().Next(127)));

        // when
        var result = () => new Postcode(value);

        // then
        result.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage("Postcode must be 100 characters or less");
    }

    [Theory]
    [InlineData("test")]
    [InlineData("PO16")]
    public void ShouldThrowException_WhenInvalid(string value)
    {
        // given & when
        var result = () => new Postcode(value);

        // then
        result.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(OrganisationErrorMessages.InvalidOrganisationPostcode);
    }
}
