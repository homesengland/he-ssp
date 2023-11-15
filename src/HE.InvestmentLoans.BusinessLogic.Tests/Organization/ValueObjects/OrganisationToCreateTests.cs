using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.Investments.Account.Domain.Organisation.Entities;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Messages;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Organization.ValueObjects;

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
    [InlineData(TestData.StringLenght101, GenericValidationError.TextTooLong)]
    public void ShouldThrowException_WhenNameIsInvalid(string name, string expectedErrorMessage)
    {
        // given & when
        var result = () => new OrganisationEntity(OrganisationNameTests.CreateName(name), OrganisationAddressTests.CreateAddress());

        // then
        result.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(expectedErrorMessage);
    }
}
