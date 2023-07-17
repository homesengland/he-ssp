using FluentAssertions;
using HE.InvestmentLoans.Common.Tests.TestData;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Application.ValueObjects;

[TestClass]
public class LoanApplicationIdCtorTests
{
    [TestMethod]
    public void LoanApplicationIdShouldBeCreated()
    {
        // given & when
        Func<LoanApplicationId> action = () => new LoanApplicationId(GuidTestData.GuidOne);

        // then
        action.Should().NotThrow();
    }

    [TestMethod]
    public void LoanThrowException_WhenGuidDefault()
    {
        // given & when
        Func<LoanApplicationId> action = () => new LoanApplicationId(Guid.Empty);

        // then
        action.Should().Throw<ArgumentException>();
    }
}
