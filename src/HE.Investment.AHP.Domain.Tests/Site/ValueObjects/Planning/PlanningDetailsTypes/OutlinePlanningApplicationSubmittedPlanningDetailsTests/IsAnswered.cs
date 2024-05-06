using FluentAssertions;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.Planning.PlanningDetailsTypes.OutlinePlanningApplicationSubmittedPlanningDetailsTests;

public class IsAnswered
{
    private readonly ReferenceNumber? _referenceNumber = new("123");
    private readonly RequiredFurtherSteps? _requiredFurtherSteps = new("some steps");
    private readonly ExpectedPlanningApprovalDate? _expectedPlanningApprovalDate = new(true, "1", "2", "2033");
    private readonly PlanningSubmissionDate? _planningSubmissionDate = new(true, "1", "2", "2033");

    [Fact]
    public void ShouldIsAnsweredReturnTrue_WhenAllDataProvided()
    {
        // given
        var details = new OutlinePlanningApplicationSubmittedPlanningDetails(
            _referenceNumber,
            _requiredFurtherSteps,
            _expectedPlanningApprovalDate,
            true,
            _planningSubmissionDate);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldIsAnsweredReturnFalse_WhenReferenceNumberMissing()
    {
        // given && when && then
        Test(
            null,
            _requiredFurtherSteps,
            _expectedPlanningApprovalDate,
            true,
            _planningSubmissionDate);
    }

    [Fact]
    public void ShouldIsAnsweredReturnFalse_WhenFurtherStepsMissing()
    {
        // given && when && then
        Test(
            _referenceNumber,
            null,
            _expectedPlanningApprovalDate,
            true,
            _planningSubmissionDate);
    }

    [Fact]
    public void ShouldIsAnsweredReturnFalse_WhenExpectedPlanningApprovalDateMissing()
    {
        // given && when && then
        Test(
            _referenceNumber,
            _requiredFurtherSteps,
            null,
            true,
            _planningSubmissionDate);
    }

    [Fact]
    public void ShouldIsAnsweredReturnFalse_WhenIsGrantFundingForAllHomesMissing()
    {
        // given && when && then
        Test(
            _referenceNumber,
            _requiredFurtherSteps,
            _expectedPlanningApprovalDate,
            null,
            _planningSubmissionDate);
    }

    [Fact]
    public void ShouldIsAnsweredReturnFalse_WhenPlanningSubmissionDateMissing()
    {
        // given && when && then
        Test(
            _referenceNumber,
            _requiredFurtherSteps,
            _expectedPlanningApprovalDate,
            false,
            null);
    }

    private static void Test(
        ReferenceNumber? referenceNumber = null,
        RequiredFurtherSteps? requiredFurtherSteps = null,
        ExpectedPlanningApprovalDate? expectedPlanningApprovalDate = null,
        bool? isGrantFundingForAllHomes = null,
        PlanningSubmissionDate? planningSubmissionDate = null)
    {
        // given
        var details = new OutlinePlanningApplicationSubmittedPlanningDetails(
            referenceNumber,
            requiredFurtherSteps,
            expectedPlanningApprovalDate,
            isGrantFundingForAllHomes,
            planningSubmissionDate);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeFalse();
    }
}
