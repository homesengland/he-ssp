using FluentAssertions;
using HE.Investments.Account.Domain.Organisation.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using Xunit;

namespace HE.Investments.Account.Domain.Tests.Organisation.ValueObjects;

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
        result.Should()
            .Throw<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == OrganisationErrorMessages.MissingOrganisationPostCode);
    }

    [Fact]
    public void ShouldThrowException_WhenTooLong()
    {
        // given
        var value = string.Join(string.Empty, Enumerable.Repeat(0, 101).Select(_ => (char)new Random().Next(127)));

        // when
        var result = () => new Postcode(value);

        // then
        result.Should()
            .Throw<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == "The Postcode must be 100 characters or less");
    }

    [Theory]
    [InlineData("test")]
    [InlineData("PO16")]
    public void ShouldThrowException_WhenInvalid(string value)
    {
        // given & when
        var result = () => new Postcode(value);

        // then
        result.Should()
            .Throw<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == OrganisationErrorMessages.InvalidOrganisationPostcode);
    }
}
