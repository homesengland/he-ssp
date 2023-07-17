using FluentAssertions;
using HE.InvestmentLoans.BusinessLogic.Application.ValueObjects;
using HE.InvestmentLoans.Common.Tests.TestData;

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
