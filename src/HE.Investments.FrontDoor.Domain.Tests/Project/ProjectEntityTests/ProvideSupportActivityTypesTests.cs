using FluentAssertions;
using HE.Investments.FrontDoor.Contract.Project.Enums;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Domain.Tests.Project.TestDataBuilders;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Project.ProjectEntityTests;

public class ProvideSupportActivityTypesTests
{
    [Fact]
    public void ShouldResetAffordableHomesQuestion_WhenSupportActivityHasChanged()
    {
        // given
        var project = ProjectEntityBuilder.New().WithSupportActivitiesAsDevelopingHomes().WithAffordableHomesAmount().Build();

        // when
        project.ProvideSupportActivityTypes(new SupportActivities(new[] { SupportActivityType.AcquiringLand }));

        // then
        project.AffordableHomesAmount.Should().Be(ProjectAffordableHomesAmount.Empty());
    }

    [Fact]
    public void ShouldNotResetAffordableHomesQuestion_WhenSupportActivityHasChangedToTheSame()
    {
        // given
        var project = ProjectEntityBuilder.New().WithSupportActivitiesAsDevelopingHomes().WithAffordableHomesAmount().Build();

        // when
        project.ProvideSupportActivityTypes(new SupportActivities(new[] { SupportActivityType.DevelopingHomes }));

        // then
        project.AffordableHomesAmount.Should().NotBe(ProjectAffordableHomesAmount.Empty());
    }
}
