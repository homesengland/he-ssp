using HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.LoanApplicationEntityTests;
public class CanBeSubmittedTests
{
    [Fact]
    public void CannotBeSubmitted_WhenCompanyStructureSectionIsNotCompleted()
    {
        var entity = LoanApplicationTestBuilder
            .NewDraft()
            .WithAllCompletedSections()
            .WithCompanyStructureSection(LoanApplicationSectionTestData.IncompleteSection)
            .Build();

        entity.CanBeSubmitted().Should().BeFalse();
    }

    [Fact]
    public void CannotBeSubmitted_WhenSecuritySectionIsNotCompleted()
    {
        var entity = LoanApplicationTestBuilder
            .NewDraft()
            .WithAllCompletedSections()
            .WithSecuritySection(LoanApplicationSectionTestData.IncompleteSection)
            .Build();

        entity.CanBeSubmitted().Should().BeFalse();
    }

    [Fact]
    public void CannotBeSubmitted_WhenFundingSectionIsNotCompleted()
    {
        var entity = LoanApplicationTestBuilder
            .NewDraft()
            .WithAllCompletedSections()
            .WithFundingSection(LoanApplicationSectionTestData.IncompleteSection)
            .Build();

        entity.CanBeSubmitted().Should().BeFalse();
    }

    [Fact]
    public void CannotBeSubmitted_WhenProjectSectionIsNotCompleted()
    {
        var entity = LoanApplicationTestBuilder
            .NewDraft()
            .WithAllCompletedSections()
            .WithProjectSection(LoanApplicationSectionTestData.IncompleteProjectsSection)
            .Build();

        entity.CanBeSubmitted().Should().BeFalse();
    }

    [Fact]
    public void CannotBeSubmitted_WhenProjectWasSubmittedAlready()
    {
        var entity = LoanApplicationTestBuilder
            .NewSubmitted()
            .Build();

        entity.CanBeSubmitted().Should().BeFalse();
    }

    [Fact]
    public void CanBeSubmitted_WhenAllSectionsAreCompleteAndApplicationIsInDraftStatus()
    {
        var entity = LoanApplicationTestBuilder
            .NewDraft()
            .WithAllCompletedSections()
            .Build();

        entity.CanBeSubmitted().Should().BeTrue();
    }
}
