using FluentAssertions;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.Planning.PlanningDetailsTypes.NoPlanningRequiredPlanningDetailsTests;

public class IsAnswered
{
    private readonly LandRegistryDetails? _landRegistryDetails = new(true, new LandRegistryTitleNumber("title"), false);

    [Fact]
    public void ShouldIsAnsweredReturnTrue_WhenAllDataProvided()
    {
        // given
        var details = new NoPlanningRequiredPlanningDetails(_landRegistryDetails);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldIsAnsweredReturnFalse_WhenDataMissing()
    {
        // given
        var details = new NoPlanningRequiredPlanningDetails();

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeFalse();
    }
}
