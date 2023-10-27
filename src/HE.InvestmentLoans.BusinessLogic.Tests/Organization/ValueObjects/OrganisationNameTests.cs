using HE.InvestmentLoans.BusinessLogic.Organization.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Constants;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Organization.ValueObjects;

public class OrganisationNameTests
{
    private static readonly string Name = "Test organisation";

    public static OrganisationName CreateName(string? name = null)
    {
        return new OrganisationName(
            name ?? Name);
    }

    [Fact]
    public void ShouldCreate()
    {
        // given
        var organisationName = "new organisation";

        // & when
        var name = new OrganisationName(organisationName);

        // then
        name.Value.Should().Be(organisationName);
    }

    [Fact]
    public void ShouldThrowException_WhenProvidedNameIsToLong()
    {
        // given
        var organisationName = TestData.StringLenght101;

        // when
        var result = () => CreateName(organisationName);

        // then
        result.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(GenericValidationError.TextTooLong);
    }
}
