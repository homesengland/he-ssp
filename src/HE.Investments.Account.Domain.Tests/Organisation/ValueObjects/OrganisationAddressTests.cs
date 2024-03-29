using FluentAssertions;
using HE.Investments.Account.Domain.Organisation.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using Xunit;

namespace HE.Investments.Account.Domain.Tests.Organisation.ValueObjects;

public class OrganisationAddressTests
{
    private static readonly string AddressLine1 = "Szybka";
    private static readonly string AddressLine2 = "2a";
    private static readonly string AddressLine3 = "5";
    private static readonly string TownOrCity = "Chorzów";
    private static readonly string Postcode = "PO16 7GZ";
    private static readonly string County = "test";
    private static readonly string Country = "UK";

    public static OrganisationAddress CreateAddress(
        string? addressLine1 = null,
        string? addressLine2 = null,
        string? addressLine3 = null,
        string? townOrCity = null,
        string? postcode = null,
        string? county = null,
        string? country = null)
    {
        return new OrganisationAddress(
            addressLine1 ?? AddressLine1,
            addressLine2 ?? AddressLine2,
            addressLine3 ?? AddressLine3,
            townOrCity ?? TownOrCity,
            postcode ?? Postcode,
            county ?? County,
            country ?? Country);
    }

    [Fact]
    public void ShouldCreate()
    {
        // given & when
        var result = CreateAddress();

        // then
        result.Should().NotBeNull();
        result.AddressLine1.Should().Be(AddressLine1);
        result.AddressLine2.Should().Be(AddressLine2);
        result.AddressLine3.Should().Be(AddressLine3);
        result.TownOrCity.Should().Be(TownOrCity);
        result.County.Should().Be(County);
        result.Country.Should().Be(Country);
        result.Postcode.Value.Should().Be(Postcode);
    }

    [Fact]
    public void ShouldCreate_WhenOptionalValuesNotProvided()
    {
        // given & when
        var result = CreateAddressWithMissingOptionalFields();

        // then
        result.Should().NotBeNull();
        result.AddressLine1.Should().Be(AddressLine1);
        result.AddressLine2.Should().BeNull();
        result.AddressLine3.Should().BeNull();
        result.TownOrCity.Should().Be(TownOrCity);
        result.County.Should().BeNull();
        result.Country.Should().BeNull();
        result.Postcode.Value.Should().Be(Postcode);
    }

    [Theory]
    [InlineData("", OrganisationErrorMessages.MissingOrganisationAddress)]
    [InlineData(TestData.StringLenght101, "The Address Line 1 must be 100 characters or less")]
    public void ShouldThrowException_WhenAddressLine1IsInvalid(string addressLine1, string expectedErrorMessage)
    {
        // given & when
        var result = () => CreateAddress(addressLine1);

        // then
        result.Should().Throw<DomainValidationException>().Which.OperationResult.Errors.Should().ContainSingle(x => x.ErrorMessage == expectedErrorMessage);
    }

    [Theory]
    [InlineData("", OrganisationErrorMessages.MissingOrganisationTownOrCity)]
    [InlineData(TestData.StringLenght101, "The Town or City must be 100 characters or less")]
    public void ShouldThrowException_WhenTownOrCityIsInvalid(string townOrCity, string expectedErrorMessage)
    {
        // given & when
        var result = () => CreateAddress(townOrCity: townOrCity);

        // then
        result.Should().Throw<DomainValidationException>().Which.OperationResult.Errors.Should().ContainSingle(x => x.ErrorMessage == expectedErrorMessage);
    }

    private OrganisationAddress CreateAddressWithMissingOptionalFields()
    {
        return new OrganisationAddress(
            AddressLine1,
            null,
            null,
            TownOrCity,
            Postcode,
            null,
            null);
    }
}
