using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Funding.ValueObjects;
using HE.Investments.Common.Exceptions;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Funding.ValueObjects;
public class RepayCtorTests
{
    [Fact]
    public void ShouldCreateRepay()
    {
        // given
        var repay = FundingFormOption.Repay;

        // when
        var action = () => Repay.New(repay);

        // then
        action.Should().NotThrow<DomainValidationException>();
        action().Value.Should().Be(repay);
    }
}
