using FluentAssertions;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Domain.Tests.Project.TestDataBuilders;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Project.ProjectEntityTests;

public class ProvideSupportActivityTypesTests
{
    [Fact]
    public void ShouldResetAffordableHomesQuestion_WhenSupportActivityHasChanged()
    {
        // given
        var project = ProjectEntityBuilder.New().WithSupportActivities(new List<SupportActivityType>
        {
            SupportActivityType.DevelopingHomes,
        }).WithAffordableHomesAmount(AffordableHomesAmount.OnlyAffordableHomes).Build();

        // when
        project.ProvideSupportActivityTypes(new SupportActivities(new[] { SupportActivityType.AcquiringLand }));

        // then
        project.AffordableHomesAmount.Should().Be(ProjectAffordableHomesAmount.Empty());
    }

    [Fact]
    public void ShouldResetProvidingInfrastructureQuestion_WhenSupportActivityHasChanged()
    {
        // given
        var project = ProjectEntityBuilder.New().WithSupportActivities(new List<SupportActivityType>
        {
            SupportActivityType.DevelopingHomes,
        }).WithAffordableHomesAmount(AffordableHomesAmount.OnlyAffordableHomes).Build();

        // when
        project.ProvideSupportActivityTypes(new SupportActivities(new[] { SupportActivityType.AcquiringLand }));

        // then
        project.Infrastructure.Should().Be(ProjectInfrastructure.Empty());
    }

    [Fact]
    public void ShouldNotResetAffordableHomesQuestion_WhenSupportActivityHasChangedToTheSame()
    {
        // given
        var project = ProjectEntityBuilder.New().WithSupportActivities(new List<SupportActivityType>
        {
            SupportActivityType.DevelopingHomes,
        }).WithAffordableHomesAmount(AffordableHomesAmount.OnlyAffordableHomes).Build();

        // when
        project.ProvideSupportActivityTypes(new SupportActivities(new[] { SupportActivityType.DevelopingHomes }));

        // then
        project.AffordableHomesAmount.Should().NotBe(ProjectAffordableHomesAmount.Empty());
    }
}
