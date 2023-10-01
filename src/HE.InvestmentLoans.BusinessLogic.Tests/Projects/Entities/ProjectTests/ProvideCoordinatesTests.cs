using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData;
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
}
