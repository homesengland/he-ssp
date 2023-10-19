using HE.InvestmentLoans.BusinessLogic.Organization.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Constants;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Organization.ValueObjects;

public class OrganisationToCreateTests
{
    private readonly string _name = "Organizacja narodów Antonia";

    [Fact]
    public void ShouldCreate()
    {
        // given & when
        var result = new OrganisationToCreate(_name, OrganisationAddressTests.CreateAddress());

        // then
        result.Name.Should().Be(_name);
    }

    [Theory]
    [InlineData("", OrganisationErrorMessages.MissingOrganisationName)]
    [InlineData(TestData.StringLenght101, GenericValidationError.TextTooLong)]
    public void ShouldThrowException_WhenNameIsInvalid(string name, string expectedErrorMessage)
    {
        // given & when
        var result = () => new OrganisationToCreate(name, OrganisationAddressTests.CreateAddress());

        // then
        result.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(expectedErrorMessage);
    }
}
