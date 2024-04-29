using FluentAssertions;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.Planning.PlanningDetailsTypes.OutlinePlanningApprovalGrantedPlanningDetailsTests;

public class IsAnswered
{
    private readonly ReferenceNumber? _referenceNumber = new("123");
    private readonly RequiredFurtherSteps? _requiredFurtherSteps = new("some steps");
    private readonly ExpectedPlanningApprovalDate? _expectedPlanningApprovalDate = new(true, "1", "2", "2033");
    private readonly OutlinePlanningApprovalDate? _outlinePlanningApprovalDate = new(true, "1", "2", "2033");

    [Fact]
    public void ShouldIsAnsweredReturnTrue_WhenAllDataProvided()
    {
        // given
        var details = new OutlinePlanningApprovalGrantedPlanningDetails(
            _referenceNumber,
            _requiredFurtherSteps,
            _expectedPlanningApprovalDate,
            _outlinePlanningApprovalDate,
            true);

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
            _outlinePlanningApprovalDate,
            true);
    }

    [Fact]
    public void ShouldIsAnsweredReturnFalse_WhenFurtherStepsMissing()
    {
        // given && when && then
        Test(
            _referenceNumber,
            null,
            _expectedPlanningApprovalDate,
            _outlinePlanningApprovalDate,
            true);
    }

    [Fact]
    public void ShouldIsAnsweredReturnFalse_WhenExpectedPlanningApprovalDateMissing()
    {
        // given && when && then
        Test(
            _referenceNumber,
            _requiredFurtherSteps,
            null,
            _outlinePlanningApprovalDate,
            true);
    }

    [Fact]
    public void ShouldIsAnsweredReturnFalse_WhenOutlinePlanningApprovalDateMissing()
    {
        // given && when && then
        Test(
            _referenceNumber,
            _requiredFurtherSteps,
            _expectedPlanningApprovalDate,
            null,
            false);
    }

    [Fact]
    public void ShouldIsAnsweredReturnFalse_WhenIsGrantFundingForAllHomesMissing()
    {
        // given && when && then
        Test(
            _referenceNumber,
            _requiredFurtherSteps,
            _expectedPlanningApprovalDate,
            _outlinePlanningApprovalDate,
            null);
    }

    private static void Test(
        ReferenceNumber? referenceNumber = null,
        RequiredFurtherSteps? requiredFurtherSteps = null,
        ExpectedPlanningApprovalDate? expectedPlanningApprovalDate = null,
        OutlinePlanningApprovalDate? outlinePlanningApprovalDate = null,
        bool? isGrantFundingForAllHomes = null)
    {
        // given
        var details = new OutlinePlanningApprovalGrantedPlanningDetails(
            referenceNumber,
            requiredFurtherSteps,
            expectedPlanningApprovalDate,
            outlinePlanningApprovalDate,
            isGrantFundingForAllHomes);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeFalse();
    }
}
