using HE.Investments.Loans.Common.Tests.TestData;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.Tests.Application.ValueObjects;

[TestClass]
public class LoanApplicationIdCtorTests
{
    [TestMethod]
    public void LoanApplicationIdShouldBeCreated()
    {
        // given & when
        var action = () => new LoanApplicationId(GuidTestData.GuidOne);

        // then
        action.Should().NotThrow();
    }

    [TestMethod]
    public void LoanApplicationIdShouldBeCreatedAsNew_WhenGuidDefault()
    {
        // given & when
        var action = () => new LoanApplicationId(Guid.Empty);

        // then
        action.Should().NotThrow();
        action().IsSaved().Should().BeFalse();
    }
}
