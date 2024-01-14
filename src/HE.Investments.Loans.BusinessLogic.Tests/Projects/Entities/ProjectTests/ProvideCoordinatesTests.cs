using HE.Investments.Common.Contract;
using HE.Investments.Loans.BusinessLogic.Projects.Entities;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.Entities.ProjectTests;
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
