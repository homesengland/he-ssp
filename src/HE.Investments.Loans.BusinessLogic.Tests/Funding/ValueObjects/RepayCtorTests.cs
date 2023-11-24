using HE.Investments.Common.Exceptions;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.Contract.Funding.ValueObjects;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Funding.ValueObjects;
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
