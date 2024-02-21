using FluentAssertions;
using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.WWW.Tests.Workflows.ApplicationWorkflowTests;

public class StateCanBeAccessedTests
{
    [Theory]
    [InlineData(ApplicationStatus.Draft, ApplicationWorkflowState.Start, true)]
    [InlineData(ApplicationStatus.Draft, ApplicationWorkflowState.ApplicationsList, true)]
    [InlineData(ApplicationStatus.Draft, ApplicationWorkflowState.ApplicationTenure, true)]
    [InlineData(ApplicationStatus.Draft, ApplicationWorkflowState.TaskList, true)]
    [InlineData(ApplicationStatus.Draft, ApplicationWorkflowState.OnHold, true)]
    [InlineData(ApplicationStatus.Draft, ApplicationWorkflowState.Reactivate, false)]
    [InlineData(ApplicationStatus.Draft, ApplicationWorkflowState.Withdraw, true)]
    [InlineData(ApplicationStatus.Draft, ApplicationWorkflowState.CheckAnswers, false)]
    [InlineData(ApplicationStatus.ApplicationSubmitted, ApplicationWorkflowState.CheckAnswers, false)]
    [InlineData(ApplicationStatus.ApplicationSubmitted, ApplicationWorkflowState.Submit, false)]
    [InlineData(ApplicationStatus.ApplicationSubmitted, ApplicationWorkflowState.Completed, false)]
    [InlineData(ApplicationStatus.ApplicationSubmitted, ApplicationWorkflowState.Withdraw, true)]
    [InlineData(ApplicationStatus.ApplicationSubmitted, ApplicationWorkflowState.OnHold, true)]
    [InlineData(ApplicationStatus.ApplicationSubmitted, ApplicationWorkflowState.Reactivate, false)]
    [InlineData(ApplicationStatus.OnHold, ApplicationWorkflowState.Reactivate, true)]
    [InlineData(ApplicationStatus.OnHold, ApplicationWorkflowState.Withdraw, true)]
    [InlineData(ApplicationStatus.OnHold, ApplicationWorkflowState.OnHold, false)]
    [InlineData(ApplicationStatus.OnHold, ApplicationWorkflowState.CheckAnswers, false)]
    [InlineData(ApplicationStatus.Withdrawn, ApplicationWorkflowState.OnHold, false)]
    [InlineData(ApplicationStatus.Withdrawn, ApplicationWorkflowState.Withdraw, false)]
    [InlineData(ApplicationStatus.Withdrawn, ApplicationWorkflowState.Reactivate, false)]
    [InlineData(ApplicationStatus.Withdrawn, ApplicationWorkflowState.CheckAnswers, false)]
    [InlineData(ApplicationStatus.Draft, ApplicationWorkflowState.RequestToEdit, false)]
    [InlineData(ApplicationStatus.ApplicationSubmitted, ApplicationWorkflowState.RequestToEdit, true)]
    [InlineData(ApplicationStatus.OnHold, ApplicationWorkflowState.RequestToEdit, false)]
    [InlineData(ApplicationStatus.Withdrawn, ApplicationWorkflowState.RequestToEdit, false)]
    [InlineData(ApplicationStatus.UnderReview, ApplicationWorkflowState.RequestToEdit, true)]
    [InlineData(ApplicationStatus.ApplicationUnderReview, ApplicationWorkflowState.RequestToEdit, true)]
    [InlineData(ApplicationStatus.CashflowUnderReview, ApplicationWorkflowState.RequestToEdit, true)]
    public async Task ShouldReturnValue_WhenMethodCalledForDefaultsWithCertainApplicationStatus(ApplicationStatus applicationStatus, ApplicationWorkflowState state, bool expectedResult)
    {
        // given
        var workflow = ApplicationWorkflowFactory.BuildWorkflow(ApplicationWorkflowState.Start, status: applicationStatus);

        // when
        var result = await workflow.StateCanBeAccessed(state);

        // then
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(ApplicationWorkflowState.Start, true)]
    [InlineData(ApplicationWorkflowState.ApplicationsList, true)]
    [InlineData(ApplicationWorkflowState.ApplicationTenure, true)]
    [InlineData(ApplicationWorkflowState.TaskList, true)]
    [InlineData(ApplicationWorkflowState.OnHold, true)]
    [InlineData(ApplicationWorkflowState.Reactivate, false)]
    [InlineData(ApplicationWorkflowState.RequestToEdit, false)]
    [InlineData(ApplicationWorkflowState.Withdraw, true)]
    [InlineData(ApplicationWorkflowState.CheckAnswers, true)]
    [InlineData(ApplicationWorkflowState.Submit, true)]
    [InlineData(ApplicationWorkflowState.Completed, true)]
    public async Task ShouldReturnValue_WhenApplicationIsCompletelyFilled(ApplicationWorkflowState state, bool expectedResult)
    {
        // given
        var sectionsWithCompletedStatus = new List<ApplicationSection>()
        {
            new(SectionType.Scheme, SectionStatus.Completed),
            new(SectionType.HomeTypes, SectionStatus.Completed),
            new(SectionType.FinancialDetails, SectionStatus.Completed),
            new(SectionType.DeliveryPhases, SectionStatus.Completed),
        };
        var workflow = ApplicationWorkflowFactory.BuildWorkflow(ApplicationWorkflowState.Start, status: ApplicationStatus.Draft, sections: sectionsWithCompletedStatus);

        // when
        var result = await workflow.StateCanBeAccessed(state);

        // then
        result.Should().Be(expectedResult);
    }
}
