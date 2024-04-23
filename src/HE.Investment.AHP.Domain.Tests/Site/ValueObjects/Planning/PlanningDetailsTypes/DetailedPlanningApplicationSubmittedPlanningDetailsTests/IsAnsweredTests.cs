using FluentAssertions;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.Planning.PlanningDetailsTypes.DetailedPlanningApplicationSubmittedPlanningDetailsTests;

public class IsAnsweredTests
{
    private readonly ReferenceNumber? _referenceNumber = new("123");
    private readonly RequiredFurtherSteps? _requiredFurtherSteps = new("some steps");

    private readonly ApplicationForDetailedPlanningSubmittedDate? _applicationForDetailedPlanningSubmittedDate =
        new(true, "1", "2", "2024");

    private readonly ExpectedPlanningApprovalDate? _expectedPlanningApprovalDate = new(true, "2", "3", "2033");

    [Fact]
    public void ShouldReturnTrue_WhenAllDataProvided()
    {
        // given
        var details = new DetailedPlanningApplicationSubmittedPlanningDetails(
            _referenceNumber,
            _requiredFurtherSteps,
            _applicationForDetailedPlanningSubmittedDate,
            _expectedPlanningApprovalDate,
            true);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalse_WhenReferenceNumberMissing()
    {
        // given && when && then
        Test(
            null,
            _requiredFurtherSteps,
            _applicationForDetailedPlanningSubmittedDate,
            _expectedPlanningApprovalDate,
            true);
    }

    [Fact]
    public void ShouldReturnFalse_WhenFurtherStepsMissing()
    {
        // given && when && then
        Test(
            _referenceNumber,
            null,
            _applicationForDetailedPlanningSubmittedDate,
            _expectedPlanningApprovalDate,
            true);
    }

    [Fact]
    public void ShouldReturnFalse_WhenApplicationForDetailedPlanningSubmittedDateMissing()
    {
        // given && when && then
        Test(
            _referenceNumber,
            _requiredFurtherSteps,
            null,
            _expectedPlanningApprovalDate,
            true);
    }

    [Fact]
    public void ShouldReturnFalse_WhenExpectedPlanningApprovalDateMissing()
    {
        // given && when && then
        Test(
            _referenceNumber,
            _requiredFurtherSteps,
            _applicationForDetailedPlanningSubmittedDate,
            null,
            true);
    }

    [Fact]
    public void ShouldReturnFalse_WhenIsGrantFundingForAllHomesMissing()
    {
        // given && when && then
        Test(
            _referenceNumber,
            _requiredFurtherSteps,
            _applicationForDetailedPlanningSubmittedDate,
            _expectedPlanningApprovalDate);
    }

    private static void Test(
        ReferenceNumber? referenceNumber = null,
        RequiredFurtherSteps? requiredFurtherSteps = null,
        ApplicationForDetailedPlanningSubmittedDate? applicationForDetailedPlanningSubmittedDate = null,
        ExpectedPlanningApprovalDate? expectedPlanningApprovalDate = null,
        bool? isGrantFundingForAllHomes = null)
    {
        // given
        var details = new DetailedPlanningApplicationSubmittedPlanningDetails(
            referenceNumber,
            requiredFurtherSteps,
            applicationForDetailedPlanningSubmittedDate,
            expectedPlanningApprovalDate,
            isGrantFundingForAllHomes);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeFalse();
    }
}
