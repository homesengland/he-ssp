using HE.Investments.Common.Domain;
using HE.Investments.Loans.BusinessLogic.Projects.Entities;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.Entities.ProjectTests;
public class ProvideLandRegistryNumber
{
    [Fact]
    public void SetLandRegistryTitleNumber()
    {
        var project = new Project();

        project.ProvideLandRegistryNumber(LocationTestData.AnyLandRegistryTitleNumber);

        project.LandRegistryTitleNumber.Should().Be(LocationTestData.AnyLandRegistryTitleNumber);
    }

    [Fact]
    public void RemoveCoordinates()
    {
        var project = new Project();

        project.ProvideCoordinates(LocationTestData.AnyCoordinates);

        project.ProvideLandRegistryNumber(LocationTestData.AnyLandRegistryTitleNumber);

        project.Coordinates.Should().BeNull();
    }

    [Fact]
    public void ShouldChangeStatusToInProgress()
    {
        // given
        var project = new Project();

        // when
        project.ProvideLandRegistryNumber(LocationTestData.AnyLandRegistryTitleNumber);

        // then
        project.Status.Should().Be(SectionStatus.InProgress);
    }
}
