using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Funding.ValueObjects;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Messages;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Funding.ValueObjects;
public class RepaymentSystemCtorTests
{
    [Fact]
    public void ShouldCreateRepaymentSystemWithRefinanceAndRefinanceAdditionalInformation()
    {
        // given
        var repaymentSystem = FundingFormOption.Refinance;
        var additionalInformation = "additional information";

        // when
        var action = () => RepaymentSystem.New(repaymentSystem, additionalInformation);

        // then
        action.Should().NotThrow<DomainValidationException>();
        action().Refinance!.Value.Should().Be(repaymentSystem);
        action().Refinance!.AdditionalInformation.Should().Be(additionalInformation);
        action().Repay.Should().BeNull();
    }

    [Fact]
    public void ShouldCreateRepaymentSystemWithRepay()
    {
        // given
        var repaymentSystem = FundingFormOption.Repay;

        // when
        var action = () => RepaymentSystem.New(repaymentSystem, null);

        // then
        action.Should().NotThrow<DomainValidationException>();
        action().Repay!.Value.Should().Be(repaymentSystem);
        action().Refinance.Should().BeNull();
    }

    [Fact]
    public void ShouldThrowArgumentException_WhenInvalidValueIsProvided()
    {
        // given
        var repaymentSystem = "not valid form option";

        // when
        var action = () => RepaymentSystem.New(repaymentSystem, null);

        // then
        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.InvalidValue);
    }
}
