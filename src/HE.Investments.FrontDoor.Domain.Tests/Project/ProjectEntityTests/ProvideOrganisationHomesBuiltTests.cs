using FluentAssertions;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Domain.Tests.Project.TestDataBuilders;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Project.ProjectEntityTests;

public class ProvideOrganisationHomesBuiltTests
{
    [Fact]
    public void ShouldChangeIsSiteIdentified_WhenIsSiteIdentifiedIsProvided()
    {
        // given
        var project = ProjectEntityBuilder.New().Build();
        var organisationHomesBuilt = new OrganisationHomesBuilt("50");

        // when
        project.ProvideOrganisationHomesBuilt(organisationHomesBuilt);

        // then
        project.OrganisationHomesBuilt.Should().Be(organisationHomesBuilt);
    }
}
