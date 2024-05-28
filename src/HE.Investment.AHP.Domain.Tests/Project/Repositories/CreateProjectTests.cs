using FluentAssertions;
using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Domain.Project.Repositories;
using HE.Investment.AHP.Domain.Tests.Common.TestDataBuilders;
using HE.Investment.AHP.Domain.Tests.Project.TestData;
using HE.Investment.AHP.Domain.Tests.Project.TestDataBuilders;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.Project.Repositories;

public class CreateProjectTests : TestBase<ProjectRepository>
{
    [Fact]
    public async Task ShouldCreateProject()
    {
        // given
        var prefillProject = ProjectPrefillDataTestData.FirstProjectPrefillData;
        var ahpProjectId = AhpProjectId.From(prefillProject.Id.Value);

        var userAccount = AhpUserContextTestBuilder
            .New()
            .Register(this)
            .AhpUserFromMock;

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
