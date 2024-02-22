using FluentAssertions;
using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investment.AHP.WWW.Tests.Workflows.ApplicationWorkflowTests;

public class NextStateTests
{
    [Theory]
    [InlineData(ApplicationWorkflowState.CheckAnswers, ApplicationWorkflowState.Submit)]
    [InlineData(ApplicationWorkflowState.Submit, ApplicationWorkflowState.Completed)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecuted(ApplicationWorkflowState current, ApplicationWorkflowState expectedNext)
    {
        // given
        var sectionsWithCompletedStatus = new List<ApplicationSection>()
        {
            new(SectionType.Scheme, SectionStatus.Completed),
            new(SectionType.HomeTypes, SectionStatus.Completed),
            new(SectionType.FinancialDetails, SectionStatus.Completed),
            new(SectionType.DeliveryPhases, SectionStatus.Completed),
        };
        var workflow = ApplicationWorkflowFactory.BuildWorkflow(current, status: ApplicationStatus.Draft, sections: sectionsWithCompletedStatus);

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(ApplicationWorkflowState.Submit, ApplicationWorkflowState.CheckAnswers)]
    [InlineData(ApplicationWorkflowState.CheckAnswers, ApplicationWorkflowState.TaskList)]
    public async Task ShouldReturnNextState_WhenBackTriggerExecuted(ApplicationWorkflowState current, ApplicationWorkflowState expectedNext)
    {
        // given
        var sectionsWithCompletedStatus = new List<ApplicationSection>()
        {
            new(SectionType.Scheme, SectionStatus.Completed),
            new(SectionType.HomeTypes, SectionStatus.Completed),
            new(SectionType.FinancialDetails, SectionStatus.Completed),
            new(SectionType.DeliveryPhases, SectionStatus.Completed),
        };
        var workflow = ApplicationWorkflowFactory.BuildWorkflow(current, status: ApplicationStatus.Draft, sections: sectionsWithCompletedStatus);

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(expectedNext);
    }
}
