using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Funding.ValueObjects;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Funding.ValueObjects;
public class RefinanceCtorTests
{
    [Fact]
    public void ShouldCreateRefinanceWithAdditionalInformation()
    {
        // given
        var refinance = FundingFormOption.Refinance;
        var additionalInformation = "additional information";

        // when
        var action = () => Refinance.New(refinance, additionalInformation);

        // then
        action.Should().NotThrow<DomainValidationException>();
        action().Value.Should().Be(refinance);
        action().AdditionalInformation.Should().Be(additionalInformation);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenRefinanceAdditionalInformationIsNotProvided()
    {
        // given
        var refinance = FundingFormOption.Refinance;
        string? additionalInformation = null;

        // when
        var action = () => Refinance.New(refinance, additionalInformation);

        // then
        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.EnterMoreDetails);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenRefinanceAdditionalInformationInputIsLongerThan1500Chars()
    {
        // given
        var refinance = FundingFormOption.Refinance;
        var additionalInformation = new string('*', 1501);

        // when
        var action = () => Refinance.New(refinance, additionalInformation);

        // then
        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.RefinanceInfo));
    }
}
