using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.ValueObjects;

public class OrganisationMoreInformationCtorTests
{
    [Theory]
    [InlineData("123       ")]
    [InlineData("Lorem Ipsum is simply dummy text of the printing and typesetting industry")]
    public void ShouldCreateOrganisationMoreInformation(string moreInformation)
    {
        // given & when
        var action = () => new OrganisationMoreInformation(moreInformation);

        // then
        action.Should().NotThrow<DomainValidationException>();
        action().Information.Should().Be(moreInformation.Trim());
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenInputIsLongerThan1000Chars()
    {
        // given & when
        var action = () => new OrganisationMoreInformation(new string('*', 1001));

        // then
        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.InputLongerThanThousandCharacters);
    }
}
