using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.Entities.ProjectTests;
public class ProvideLandOwnership
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
        project.AdditionalData.Should().BeNull();
    }
}
