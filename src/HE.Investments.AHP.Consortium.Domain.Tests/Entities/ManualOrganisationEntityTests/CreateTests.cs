using FluentAssertions;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.Common.Contract.Exceptions;
using Xunit;

namespace HE.Investments.AHP.Consortium.Domain.Tests.Entities.ManualOrganisationEntityTests;

public class CreateTests
{
    [Fact]
    public void ShouldThrowException_WhenNoFieldsAreProvided()
    {
        // given & when
        var create = () => ManualOrganisationEntity.Create(null, null, null, null, null, null);

        // then
        var result = create.Should().Throw<DomainValidationException>().Which.OperationResult;

        result.Errors.Should().HaveCount(4);
        result.Errors.Select(x => x.AffectedField).Should().Contain("Name").And.Contain("AddressLine1").And.Contain("TownOrCity").And.Contain("Postcode");
    }

    [Fact]
    public void ShouldThrowException_WhenAllFieldsAreTooLong()
    {
        // given
        var text = new string(Enumerable.Repeat('a', 101).ToArray());

        // when
        var create = () => ManualOrganisationEntity.Create(text, text, text, text, text, text);

        // then
        var result = create.Should().Throw<DomainValidationException>().Which.OperationResult;

        result.Errors.Should().HaveCount(6);
        result.Errors.Select(x => x.AffectedField)
            .Should()
            .Contain("Name")
            .And.Contain("AddressLine1")
            .And.Contain("AddressLine2")
            .And.Contain("TownOrCity")
            .And.Contain("County")
            .And.Contain("Postcode");
    }

    [Fact]
    public void ShouldCreateManualOrganisation_WhenAllFieldsAreCorrect()
    {
        // given & when
        var result = ManualOrganisationEntity.Create(
            "My organisation",
            "Groove street",
            "12b",
            "Fishtown",
            "Silesia",
            "HE1 2UK");

        // then
        result.Should().NotBeNull();
        result.Name.Value.Should().Be("My organisation");
        result.AddressLine1.Value.Should().Be("Groove street");
        result.AddressLine2!.Value.Should().Be("12b");
        result.TownOrCity.Value.Should().Be("Fishtown");
        result.County!.Value.Should().Be("Silesia");
        result.Postcode.Value.Should().Be("HE1 2UK");
    }
}
