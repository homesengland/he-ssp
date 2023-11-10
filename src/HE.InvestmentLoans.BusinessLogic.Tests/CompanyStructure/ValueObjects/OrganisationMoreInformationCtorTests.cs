using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Messages;
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
    public void ShouldThrowDomainValidationException_WhenInputIsLongerThan1500Chars()
    {
        // given & when
        var action = () => new OrganisationMoreInformation(new string('*', 1501));

        // then
        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.OrganisationMoreInformation));
    }
}
