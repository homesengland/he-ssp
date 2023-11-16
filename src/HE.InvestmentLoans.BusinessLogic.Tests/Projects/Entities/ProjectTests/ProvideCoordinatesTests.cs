using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData;
using HE.Investments.Common.Domain;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.Entities.ProjectTests;
public class ProvideCoordinatesTests
{
    [Fact]
    public void SetCoordinates()
    {
        var project = new Project();

        project.ProvideCoordinates(LocationTestData.AnyCoordinates);

        project.Coordinates.Should().Be(LocationTestData.AnyCoordinates);
    }

    [Fact]
    public void RemoveLandRegistryTitleNumber()
    {
        var project = new Project();

        project.ProvideLandRegistryNumber(LocationTestData.AnyLandRegistryTitleNumber);

        project.ProvideCoordinates(LocationTestData.AnyCoordinates);

        project.LandRegistryTitleNumber.Should().BeNull();
    }

    [Fact]
    public void ShouldChangeStatusToInProgress()
    {
        // given
        var project = new Project();

        // when
        project.ProvideCoordinates(LocationTestData.AnyCoordinates);

        // then
        project.Status.Should().Be(SectionStatus.InProgress);
    }
}
