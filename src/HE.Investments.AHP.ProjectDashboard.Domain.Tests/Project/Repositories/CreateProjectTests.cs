using FluentAssertions;
using HE.Investments.AHP.Consortium.Domain.Tests.TestObjectBuilders;
using HE.Investments.AHP.ProjectDashboard.Contract.Project;
using HE.Investments.AHP.ProjectDashboard.Domain.Project.Repositories;
using HE.Investments.AHP.ProjectDashboard.Domain.Tests.Project.TestData;
using HE.Investments.AHP.ProjectDashboard.Domain.Tests.Project.TestDataBuilders;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Tests.Project.Repositories;

public class CreateProjectTests : TestBase<ProjectRepository>
{
    [Fact]
    public async Task ShouldCreateProject()
    {
        // given
        var prefillProject = ProjectPrefillDataTestData.FirstProjectPrefillData;
        var ahpProjectId = AhpProjectId.From(prefillProject.Id.Value);

        var userAccount = ConsortiumUserContextTestBuilder
            .New()
            .Register(this)
            .ConsortiumUserFromMock;

        ProjectCrmContextTestBuilder
            .New()
            .CreateProject(prefillProject.Id.ToGuidAsString(), prefillProject.Name, ahpProjectId.Value)
            .BuildMockAndRegister(this);

        // when
        var result = await TestCandidate.CreateProject(prefillProject, userAccount, CancellationToken.None);

        // then
        result.Should().Be(ahpProjectId);
    }
}
