using FluentAssertions;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.Planning.PlanningDetailsTypes.DetailedPlanningApprovalGrantedPlanningDetailsTests;

public class IsAnsweredTests
{
    private readonly ReferenceNumber? _referenceNumber = new("123");
    private readonly DetailedPlanningApprovalDate? _detailedPlanningApprovalDate = new("1", "2", "2025");

    [Fact]
    public void ShouldIsAnsweredReturnTrue_WhenAllDataProvided()
    {
        // given
        var details = new DetailedPlanningApprovalGrantedPlanningDetails(
            _referenceNumber,
            _detailedPlanningApprovalDate,
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
            _detailedPlanningApprovalDate,
            true);
    }

    [Fact]
    public void ShouldIsAnsweredReturnFalse_WhenDetailedPlanningApprovalDateMissing()
    {
        // given && when && then
        Test(
            _referenceNumber,
            null,
            true);
    }

    [Fact]
    public void ShouldIsAnsweredReturnFalse_WhenIsGrantFundingForAllHomesMissing()
    {
        // given && when && then
        Test(
            _referenceNumber,
            _detailedPlanningApprovalDate);
    }

    private static void Test(
        ReferenceNumber? referenceNumber = null,
        DetailedPlanningApprovalDate? detailedPlanningApprovalDate = null,
        bool? isGrantFundingForAllHomes = null)
    {
        // given
        var details = new DetailedPlanningApprovalGrantedPlanningDetails(
            referenceNumber,
            detailedPlanningApprovalDate,
            isGrantFundingForAllHomes);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeFalse();
    }
}
