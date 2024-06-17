using FluentAssertions;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Tests.TestDataBuilders;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investment.AHP.WWW.Tests.Workflows.SiteWorkflowTests.Section106;

public class NextStateTests
{
    public static IEnumerable<object[]> ContinueTrigger()
    {
        yield return [new Section106Answers(GeneralAgreement: false), new[] { SiteWorkflowState.LocalAuthoritySearch }];

        yield return
        [
            new Section106Answers(AffordableHousing: true, OnlyAffordableHousing: false, AdditionalAffordableHousing: false, CapitalFundingEligibility: false),
            new[]
            {
                SiteWorkflowState.Section106AffordableHousing,
                SiteWorkflowState.Section106OnlyAffordableHousing,
                SiteWorkflowState.Section106AdditionalAffordableHousing,
                SiteWorkflowState.Section106CapitalFundingEligibility,
                SiteWorkflowState.Section106Ineligible,
            },
        ];

        yield return
        [
            new Section106Answers(AffordableHousing: false, CapitalFundingEligibility: true),
            new[]
            {
                SiteWorkflowState.Section106AffordableHousing,
                SiteWorkflowState.Section106CapitalFundingEligibility,
                SiteWorkflowState.Section106Ineligible,
            },
        ];

        yield return
        [
            new Section106Answers(AffordableHousing: false, CapitalFundingEligibility: false),
            new[]
            {
                SiteWorkflowState.Section106AffordableHousing,
                SiteWorkflowState.Section106CapitalFundingEligibility,
                SiteWorkflowState.LocalAuthoritySearch,
            },
        ];

        yield return
        [
            new Section106Answers(AffordableHousing: true, OnlyAffordableHousing: true, CapitalFundingEligibility: true),
            new[]
            {
                SiteWorkflowState.Section106AffordableHousing,
                SiteWorkflowState.Section106OnlyAffordableHousing,
                SiteWorkflowState.Section106CapitalFundingEligibility,
                SiteWorkflowState.Section106Ineligible,
            },
        ];

        yield return
        [
            new Section106Answers(AffordableHousing: true, OnlyAffordableHousing: true, CapitalFundingEligibility: false),
            new[]
            {
                SiteWorkflowState.Section106AffordableHousing,
                SiteWorkflowState.Section106OnlyAffordableHousing,
                SiteWorkflowState.Section106CapitalFundingEligibility,
                SiteWorkflowState.LocalAuthoritySearch,
            },
        ];

        yield return
        [
            new Section106Answers(AffordableHousing: true, OnlyAffordableHousing: false, AdditionalAffordableHousing: true, CapitalFundingEligibility: true),
            new[]
            {
                SiteWorkflowState.Section106AffordableHousing,
                SiteWorkflowState.Section106OnlyAffordableHousing,
                SiteWorkflowState.Section106AdditionalAffordableHousing,
                SiteWorkflowState.Section106CapitalFundingEligibility,
                SiteWorkflowState.Section106Ineligible,
            },
        ];

        yield return
        [
            new Section106Answers(AffordableHousing: true, OnlyAffordableHousing: false, AdditionalAffordableHousing: false, CapitalFundingEligibility: true),
            new[]
            {
                SiteWorkflowState.Section106AffordableHousing,
                SiteWorkflowState.Section106OnlyAffordableHousing,
                SiteWorkflowState.Section106AdditionalAffordableHousing,
                SiteWorkflowState.Section106CapitalFundingEligibility,
                SiteWorkflowState.Section106Ineligible,
            },
        ];

        yield return
        [
            new Section106Answers(AffordableHousing: true, OnlyAffordableHousing: false, AdditionalAffordableHousing: true, CapitalFundingEligibility: false),
            new[]
            {
                SiteWorkflowState.Section106AffordableHousing,
                SiteWorkflowState.Section106OnlyAffordableHousing,
                SiteWorkflowState.Section106AdditionalAffordableHousing,
                SiteWorkflowState.Section106CapitalFundingEligibility,
                SiteWorkflowState.Section106LocalAuthorityConfirmation,
                SiteWorkflowState.LocalAuthoritySearch,
            },
        ];
    }

    public static IEnumerable<object[]> BackTriggerNotEligible()
    {
        yield return
        [
            new Section106Answers(AffordableHousing: true, OnlyAffordableHousing: false, AdditionalAffordableHousing: false, CapitalFundingEligibility: false),
            new[]
            {
                SiteWorkflowState.Section106CapitalFundingEligibility,
                SiteWorkflowState.Section106AdditionalAffordableHousing,
                SiteWorkflowState.Section106OnlyAffordableHousing,
                SiteWorkflowState.Section106AffordableHousing,
                SiteWorkflowState.Section106GeneralAgreement,
            },
        ];

        yield return
        [
            new Section106Answers(AffordableHousing: false, CapitalFundingEligibility: true),
            new[]
            {
                SiteWorkflowState.Section106CapitalFundingEligibility,
                SiteWorkflowState.Section106AffordableHousing,
                SiteWorkflowState.Section106GeneralAgreement,
            },
        ];

        yield return
        [
            new Section106Answers(AffordableHousing: true, OnlyAffordableHousing: true, CapitalFundingEligibility: true),
            new[]
            {
                SiteWorkflowState.Section106CapitalFundingEligibility,
                SiteWorkflowState.Section106OnlyAffordableHousing,
                SiteWorkflowState.Section106AffordableHousing,
                SiteWorkflowState.Section106GeneralAgreement,
            },
        ];

        yield return
        [
            new Section106Answers(AffordableHousing: true, OnlyAffordableHousing: false, AdditionalAffordableHousing: true, CapitalFundingEligibility: true),
            new[]
            {
                SiteWorkflowState.Section106CapitalFundingEligibility,
                SiteWorkflowState.Section106AdditionalAffordableHousing,
                SiteWorkflowState.Section106OnlyAffordableHousing,
                SiteWorkflowState.Section106AffordableHousing,
                SiteWorkflowState.Section106GeneralAgreement,
            },
        ];

        yield return
        [
            new Section106Answers(AffordableHousing: true, OnlyAffordableHousing: false, AdditionalAffordableHousing: false, CapitalFundingEligibility: true),
            new[]
            {
                SiteWorkflowState.Section106CapitalFundingEligibility,
                SiteWorkflowState.Section106AdditionalAffordableHousing,
                SiteWorkflowState.Section106OnlyAffordableHousing,
                SiteWorkflowState.Section106AffordableHousing,
                SiteWorkflowState.Section106GeneralAgreement,
            },
        ];
    }

    public static IEnumerable<object[]> BackTriggerEligible()
    {
        yield return [new Section106Answers(GeneralAgreement: false), new[] { SiteWorkflowState.Section106GeneralAgreement }];

        yield return
        [
            new Section106Answers(AffordableHousing: false, CapitalFundingEligibility: false),
            new[]
            {
                SiteWorkflowState.Section106CapitalFundingEligibility,
                SiteWorkflowState.Section106AffordableHousing,
                SiteWorkflowState.Section106GeneralAgreement,
            },
        ];

        yield return
        [
            new Section106Answers(AffordableHousing: true, OnlyAffordableHousing: true, CapitalFundingEligibility: false),
            new[]
            {
                SiteWorkflowState.Section106CapitalFundingEligibility,
                SiteWorkflowState.Section106OnlyAffordableHousing,
                SiteWorkflowState.Section106AffordableHousing,
                SiteWorkflowState.Section106GeneralAgreement,
            },
        ];

        yield return
        [
            new Section106Answers(AffordableHousing: true, OnlyAffordableHousing: false, AdditionalAffordableHousing: true, CapitalFundingEligibility: false),
            new[]
            {
                SiteWorkflowState.Section106LocalAuthorityConfirmation,
                SiteWorkflowState.Section106CapitalFundingEligibility,
                SiteWorkflowState.Section106AdditionalAffordableHousing,
                SiteWorkflowState.Section106OnlyAffordableHousing,
                SiteWorkflowState.Section106AffordableHousing,
                SiteWorkflowState.Section106GeneralAgreement,
            },
        ];
    }

    [Theory]
    [MemberData(nameof(ContinueTrigger))]
    public async Task ShouldFollowCorrectFlow_WhenContinueTriggerIsExecuted(Section106Answers answers, IEnumerable<SiteWorkflowState> expectedStates)
    {
        // given
        var workflow = BuildWorkflow(SiteWorkflowState.Section106GeneralAgreement, answers);

        // when
        foreach (var expectedState in expectedStates)
        {
            var nextState = await workflow.NextState(Trigger.Continue);

            // then
            nextState.Should().Be(expectedState);
            workflow = BuildWorkflow(nextState, answers);
        }
    }

    [Theory]
    [MemberData(nameof(BackTriggerNotEligible))]
    public async Task ShouldFollowCorrectFlow_WhenBackTriggerIsExecutedAndStateIsNotEligible(Section106Answers answers, IEnumerable<SiteWorkflowState> expectedStates)
    {
        // given
        var workflow = BuildWorkflow(SiteWorkflowState.Section106Ineligible, answers);

        // when
        foreach (var expectedState in expectedStates)
        {
            var previousState = await workflow.NextState(Trigger.Back);

            // then
            previousState.Should().Be(expectedState);
            workflow = BuildWorkflow(previousState, answers);
        }
    }

    [Theory]
    [MemberData(nameof(BackTriggerEligible))]
    public async Task ShouldFollowCorrectFlow_WhenBackTriggerIsExecutedAndStateIsEligible(Section106Answers answers, IEnumerable<SiteWorkflowState> expectedStates)
    {
        // given
        var workflow = BuildWorkflow(SiteWorkflowState.LocalAuthoritySearch, answers);

        // when
        foreach (var expectedState in expectedStates)
        {
            var previousState = await workflow.NextState(Trigger.Back);

            // then
            previousState.Should().Be(expectedState);
            workflow = BuildWorkflow(previousState, answers);
        }
    }

    private static SiteWorkflow BuildWorkflow(SiteWorkflowState currentState, Section106Answers answers)
    {
        var section106 = new Section106TestDataBuilder()
            .WithGeneralAgreement(answers.GeneralAgreement)
            .WithAffordableHousing(answers.AffordableHousing)
            .WithOnlyAffordableHousing(answers.OnlyAffordableHousing)
            .WithAdditionalAffordableHousing(answers.AdditionalAffordableHousing)
            .WithCapitalFundingEligibility(answers.CapitalFundingEligibility)
            .Build();

        return new SiteWorkflow(currentState, SiteModelBuilder.Build(section106));
    }

    public sealed record Section106Answers(
        bool? GeneralAgreement = true,
        bool? AffordableHousing = null,
        bool? OnlyAffordableHousing = null,
        bool? AdditionalAffordableHousing = null,
        bool? CapitalFundingEligibility = null);
}
