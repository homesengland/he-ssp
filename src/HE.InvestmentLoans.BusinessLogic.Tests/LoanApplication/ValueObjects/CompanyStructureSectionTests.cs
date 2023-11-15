using HE.InvestmentLoans.BusinessLogic.LoanApplication.ValueObjects;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.Investments.Common.Domain;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.ValueObjects;
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
