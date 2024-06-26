using FluentAssertions;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.Planning.PlanningDetailsTypes.NoProgressOnPlanningApplicationPlanningDetailsTests;

public class IsAnswered
{
    private readonly ExpectedPlanningApprovalDate? _expectedPlanningApprovalDate = new(true, "1", "2", "2025");
    private readonly LandRegistryDetails? _landRegistryDetails = new(true, new LandRegistryTitleNumber("title"), false);

    [Fact]
    public void ShouldIsAnsweredReturnTrue_WhenAllDataProvided()
    {
        // given
        var details = new NoProgressOnPlanningApplicationPlanningDetails(_expectedPlanningApprovalDate, _landRegistryDetails);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldIsAnsweredReturnFalse_WhenExpectedPlanningApprovalDateMissing()
    {
        // given && when && then
        Test(
            null,
            _landRegistryDetails);
    }

    [Fact]
    public void ShouldIsAnsweredReturnFalse_WhenLandRegistryDetailsMissing()
    {
        // given && when && then
        Test(
            _expectedPlanningApprovalDate,
            null);
    }

    private static void Test(
        ExpectedPlanningApprovalDate? expectedPlanningApprovalDate,
        LandRegistryDetails? landRegistryDetails)
    {
        // given
        var details = new NoProgressOnPlanningApplicationPlanningDetails(
            expectedPlanningApprovalDate,
            landRegistryDetails);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeFalse();
    }
}
