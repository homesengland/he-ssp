using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestData;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.ValueObjects;
public class ProjectsSectionTests
{
    [Fact]
    public void IsNotComplete_WhenNoProjectsWereAdded()
    {
        // given
        var section = ProjectsSection.Empty();

        // when then
        section.IsCompleted().Should().BeFalse();
    }

    [Fact]
    public void IsNotComplete_WhenAnyProjectIsIncomplete()
    {
        // given
        var section = new ProjectsSection(
            Projects(
                ProjectBasicDataTestData.CompleteProject,
                ProjectBasicDataTestData.IncompleteProject));

        // when then
        section.IsCompleted().Should().BeFalse();
    }

    [Fact]
    public void IsComplete_WhenAllProjectAreComplete()
    {
        // given
        var section = new ProjectsSection(
            Projects(
                ProjectBasicDataTestData.CompleteProject,
                ProjectBasicDataTestData.CompleteProject));

        // when then
        section.IsCompleted().Should().BeTrue();
    }

    private IEnumerable<ProjectBasicData> Projects(params ProjectBasicData[] projects)
    {
        return projects;
    }
}
