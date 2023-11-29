using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;
using HE.Investments.Loans.BusinessLogic.Projects.Entities;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.Entities.ProjectTests;
public class ProvideLandOwnershipTests
{
    [Fact]
    public void ShouldSetOwnershipInformation()
    {
        // given
        var project = new Project();

        var ownership = new LandOwnership(true);

        // when
        project.ProvideLandOwnership(ownership);

        // then
        project.LandOwnership.Should().Be(ownership);
    }

    [Fact]
    public void ShouldRemoveAdditionalData_WhenUserDoesNotHaveFullOwnership()
    {
        // given
        var project = new Project();

        var ownership = new LandOwnership(false);

        // when
        project.ProvideLandOwnership(ownership);

        // then
        project.AdditionalDetails.Should().BeNull();
    }

    [Fact]
    public void ShouldChangeStatusToInProgress()
    {
        // given
        var project = new Project();

        var ownership = new LandOwnership(false);

        // when
        project.ProvideLandOwnership(ownership);

        // then
        project.Status.Should().Be(SectionStatus.InProgress);
    }
}
