using FluentAssertions;
using HE.Investments.Account.Domain.Organisation.Entities;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using Xunit;

namespace HE.Investments.Account.Domain.Tests.Organisation.ValueObjects;

public class OrganisationToCreateTests
{
    [Fact]
    public void ShouldCreate()
    {
        // given
        var name = "Organizacja narodów Antonia";

        // when
        var result = new OrganisationEntity(OrganisationNameTests.CreateName(name), OrganisationAddressTests.CreateAddress());

        // then
        result.Name.Name.Should().Be(name);
    }

    [Theory]
    [InlineData("", OrganisationErrorMessages.MissingOrganisationName)]
    [InlineData(TestData.StringLenght101, "The Organisation name must be 100 characters or less")]
    public void ShouldThrowException_WhenNameIsInvalid(string name, string expectedErrorMessage)
    {
        // given & when
        var result = () => new OrganisationEntity(OrganisationNameTests.CreateName(name), OrganisationAddressTests.CreateAddress());

        // then
        result.Should().Throw<DomainValidationException>().Which.OperationResult.Errors.Should().ContainSingle(x => x.ErrorMessage == expectedErrorMessage);
    }
}
