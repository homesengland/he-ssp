using FluentAssertions;
using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.WWW.Tests.Workflows.ApplicationWorkflowTests;

public class CanBeAccessedTests
{
    [Theory]
    [InlineData(ApplicationWorkflowState.OnHold, AhpApplicationOperation.PutOnHold)]
    [InlineData(ApplicationWorkflowState.Reactivate, AhpApplicationOperation.Reactivate)]
    [InlineData(ApplicationWorkflowState.RequestToEdit, AhpApplicationOperation.RequestToEdit)]
    [InlineData(ApplicationWorkflowState.Withdraw, AhpApplicationOperation.Withdraw)]
    public async Task ShouldReturnTrue_WhenOperationIsAllowed(ApplicationWorkflowState state, AhpApplicationOperation allowedOperation)
    {
        // given
        var workflow = ApplicationWorkflowFactory.BuildWorkflow(
            ApplicationWorkflowState.Start,
            allowedOperations: new[] { AhpApplicationOperation.Modification, allowedOperation });

        // when
        var result = await workflow.StateCanBeAccessed(state);

        // then
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(ApplicationWorkflowState.OnHold)]
    [InlineData(ApplicationWorkflowState.Reactivate)]
    [InlineData(ApplicationWorkflowState.RequestToEdit)]
    [InlineData(ApplicationWorkflowState.Withdraw)]
    public async Task ShouldReturnFalse_WhenOperationIsNotAllowed(ApplicationWorkflowState state)
    {
        // given
        var workflow = ApplicationWorkflowFactory.BuildWorkflow(
            ApplicationWorkflowState.Start,
            allowedOperations: new[] { AhpApplicationOperation.Modification });

        // when
        var result = await workflow.StateCanBeAccessed(state);

        // then
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(ApplicationWorkflowState.CheckAnswers)]
    [InlineData(ApplicationWorkflowState.Submit)]
    public async Task ShouldReturnTrue_WhenAllSectionsAreCompletedAndSubmitIsAllowed(ApplicationWorkflowState state)
    {
        // given
        var sections = new List<ApplicationSection>
        {
            new(SectionType.Scheme, SectionStatus.Completed),
            new(SectionType.HomeTypes, SectionStatus.Completed),
            new(SectionType.FinancialDetails, SectionStatus.Completed),
            new(SectionType.DeliveryPhases, SectionStatus.Completed),
        };
        var workflow = ApplicationWorkflowFactory.BuildWorkflow(
            ApplicationWorkflowState.Start,
            sections: sections,
            allowedOperations: new[] { AhpApplicationOperation.Submit });

        // when
        var result = await workflow.StateCanBeAccessed(state);

        // then
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(ApplicationWorkflowState.CheckAnswers)]
    [InlineData(ApplicationWorkflowState.Submit)]
    public async Task ShouldReturnFalse_WhenAllSectionsAreCompletedButSubmitIsNotAllowed(ApplicationWorkflowState state)
    {
        // given
        var sections = new List<ApplicationSection>
        {
            new(SectionType.Scheme, SectionStatus.Completed),
            new(SectionType.HomeTypes, SectionStatus.Completed),
            new(SectionType.FinancialDetails, SectionStatus.Completed),
            new(SectionType.DeliveryPhases, SectionStatus.Completed),
        };
        var workflow = ApplicationWorkflowFactory.BuildWorkflow(
            ApplicationWorkflowState.Start,
            sections: sections,
            allowedOperations: new[] { AhpApplicationOperation.Modification });

        // when
        var result = await workflow.StateCanBeAccessed(state);

        // then
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(ApplicationWorkflowState.CheckAnswers)]
    [InlineData(ApplicationWorkflowState.Submit)]
    public async Task ShouldReturnFalse_WhenSubmitIsAllowedButNotAllSectionsAreCompleted(ApplicationWorkflowState state)
    {
        // given
        var sections = new List<ApplicationSection>
        {
            new(SectionType.Scheme, SectionStatus.Completed),
            new(SectionType.HomeTypes, SectionStatus.Completed),
            new(SectionType.FinancialDetails, SectionStatus.Completed),
            new(SectionType.DeliveryPhases, SectionStatus.InProgress),
        };
        var workflow = ApplicationWorkflowFactory.BuildWorkflow(
            ApplicationWorkflowState.Start,
            sections: sections,
            allowedOperations: new[] { AhpApplicationOperation.Submit });

        // when
        var result = await workflow.StateCanBeAccessed(state);

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldReturnTrue_WhenApplicationIsSubmitted()
    {
        // given
        var sections = new List<ApplicationSection>
        {
            new(SectionType.Scheme, SectionStatus.Submitted),
            new(SectionType.HomeTypes, SectionStatus.Submitted),
            new(SectionType.FinancialDetails, SectionStatus.Submitted),
            new(SectionType.DeliveryPhases, SectionStatus.Submitted),
        };
        var workflow = ApplicationWorkflowFactory.BuildWorkflow(
            ApplicationWorkflowState.Start,
            status: ApplicationStatus.ApplicationSubmitted,
            sections: sections);

        // when
        var result = await workflow.StateCanBeAccessed(ApplicationWorkflowState.Completed);

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldReturnFalse_WhenAllSectionsAreSubmittedButStatusIsNotSubmitted()
    {
        // given
        var sections = new List<ApplicationSection>
        {
            new(SectionType.Scheme, SectionStatus.Submitted),
            new(SectionType.HomeTypes, SectionStatus.Submitted),
            new(SectionType.FinancialDetails, SectionStatus.Submitted),
            new(SectionType.DeliveryPhases, SectionStatus.Submitted),
        };
        var workflow = ApplicationWorkflowFactory.BuildWorkflow(ApplicationWorkflowState.Start, sections: sections, status: ApplicationStatus.Draft);

        // when
        var result = await workflow.StateCanBeAccessed(ApplicationWorkflowState.Completed);

        // then
        result.Should().BeFalse();
    }
}
