using HE.Investments.Common.Contract;
using HE.Investments.Loans.BusinessLogic.LoanApplication.ValueObjects;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.ValueObjects;
public class CompanyStructureSectionTests
{
    [Theory]
    [InlineData(SectionStatus.NotStarted)]
    [InlineData(SectionStatus.InProgress)]
    [InlineData(SectionStatus.Undefined)]
    [InlineData(SectionStatus.Withdrawn)]
    public void IsNotCompleted_WhenStatusIsDifferentThanCompleted(SectionStatus sectionStatus)
    {
        var section = new LoanApplicationSection(sectionStatus);

        section.IsCompleted().Should().BeFalse();
    }

    [Fact]
    public void IsCompleted_WhenStatusIsCompleted()
    {
        var section = new LoanApplicationSection(SectionStatus.Completed);

        section.IsCompleted().Should().BeTrue();
    }
}
