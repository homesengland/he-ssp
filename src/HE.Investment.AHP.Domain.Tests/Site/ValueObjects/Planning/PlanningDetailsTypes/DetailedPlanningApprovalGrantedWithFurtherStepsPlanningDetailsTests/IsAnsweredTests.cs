using FluentAssertions;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.Planning.PlanningDetailsTypes.DetailedPlanningApprovalGrantedWithFurtherStepsPlanningDetailsTests;

public class IsAnsweredTests
{
    private readonly ReferenceNumber? _referenceNumber = new("123");
    private readonly DetailedPlanningApprovalDate? _detailedPlanningApprovalDate = new(true, "1", "2", "2025");
    private readonly RequiredFurtherSteps? _requiredFurtherSteps = new("some steps");

    [Fact]
    public void ShouldReturnTrue_WhenAllDataProvided()
    {
        // given
        var details = new DetailedPlanningApprovalGrantedWithFurtherStepsPlanningDetails(
            _referenceNumber,
            _detailedPlanningApprovalDate,
            _requiredFurtherSteps,
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
            _detailedPlanningApprovalDate,
            _requiredFurtherSteps,
            false);
    }

    [Fact]
    public void ShouldReturnFalse_WhenDetailedPlanningApprovalDateMissing()
    {
        // given && when && then
        Test(
            _referenceNumber,
            null,
            _requiredFurtherSteps,
            true);
    }

    [Fact]
    public void ShouldReturnFalse_WhenFurtherStepsMissing()
    {
        // given && when && then
        Test(
            _referenceNumber,
            _detailedPlanningApprovalDate,
            null,
            false);
    }

    [Fact]
    public void ShouldReturnFalse_WhenIsGrantFundingForAllHomesMissing()
    {
        // given && when && then
        Test(
            _referenceNumber,
            _detailedPlanningApprovalDate,
            _requiredFurtherSteps);
    }

    private static void Test(
        ReferenceNumber? referenceNumber = null,
        DetailedPlanningApprovalDate? detailedPlanningApprovalDate = null,
        RequiredFurtherSteps? requiredFurtherSteps = null,
        bool? isGrantFundingForAllHomes = null)
    {
        // given
        var details = new DetailedPlanningApprovalGrantedWithFurtherStepsPlanningDetails(
            referenceNumber,
            detailedPlanningApprovalDate,
            requiredFurtherSteps,
            isGrantFundingForAllHomes);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeFalse();
    }
}
