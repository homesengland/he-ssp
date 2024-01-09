using HE.Investments.Account.Domain.Organisation.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Loans.BusinessLogic.Tests.Assertions;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Organization.ValueObjects;

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
        name.Name.Should().Be(organisationName);
    }

    [Fact]
    public void ShouldThrowException_WhenProvidedNameIsToLong()
    {
        // given
        var organisationName = TestData.StringLenght101;

        // when
        var result = () => CreateName(organisationName);

        // then
        result.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage("Organisation name must be 100 characters or less");
    }
}
