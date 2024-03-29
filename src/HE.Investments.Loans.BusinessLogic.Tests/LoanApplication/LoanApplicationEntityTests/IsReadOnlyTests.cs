using HE.Investments.Common.Contract;
using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Entities;
using HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.LoanApplicationEntityTests;

public class IsReadOnlyTests : TestBase<LoanApplicationEntity>
{
    [Theory]
    [InlineData(ApplicationStatus.CashflowRequested)]
    [InlineData(ApplicationStatus.CashflowUnderReview)]
    [InlineData(ApplicationStatus.UnderReview)]
    [InlineData(ApplicationStatus.SentForApproval)]
    [InlineData(ApplicationStatus.ApprovedSubjectToDueDiligence)]
    [InlineData(ApplicationStatus.ApplicationDeclined)]
    [InlineData(ApplicationStatus.InDueDiligence)]
    [InlineData(ApplicationStatus.ApprovedSubjectToContract)]
    [InlineData(ApplicationStatus.AwaitingCpSatisfaction)]
    [InlineData(ApplicationStatus.ConditionsSatisfied)]
    [InlineData(ApplicationStatus.LoanAvailable)]
    [InlineData(ApplicationStatus.HoldRequested)]
    [InlineData(ApplicationStatus.OnHold)]
    [InlineData(ApplicationStatus.Withdrawn)]
    public void ShouldReturnTrue_WhenLoanApplicationStatusIsReadOnly(ApplicationStatus status)
    {
        // given
        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var loanApplicationEntity = LoanApplicationTestBuilder.NewDraft(userAccount).Build();
        loanApplicationEntity.ExternalStatus = status;

        // when
        var result = loanApplicationEntity.IsReadOnly();

        // then
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(ApplicationStatus.Draft)]
    [InlineData(ApplicationStatus.ReferredBackToApplicant)]
    public void ShouldReturnFalse_WhenLoanApplicationStatusIsNotReadOnly(ApplicationStatus status)
    {
        // given
        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var loanApplicationEntity = LoanApplicationTestBuilder.NewDraft(userAccount).Build();
        loanApplicationEntity.ExternalStatus = status;

        // when
        var result = loanApplicationEntity.IsReadOnly();

        // then
        result.Should().BeFalse();
    }
}
