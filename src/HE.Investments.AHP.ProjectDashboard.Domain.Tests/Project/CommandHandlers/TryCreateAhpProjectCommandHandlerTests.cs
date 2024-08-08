using FluentAssertions;
using HE.Investment.AHP.Domain.Tests.Common.TestDataBuilders;
using HE.Investments.AHP.Consortium.Domain.Tests.TestObjectBuilders;
using HE.Investments.AHP.ProjectDashboard.Contract.Project;
using HE.Investments.AHP.ProjectDashboard.Contract.Project.Commands;
using HE.Investments.AHP.ProjectDashboard.Domain.Project.CommandHandlers;
using HE.Investments.AHP.ProjectDashboard.Domain.Tests.Project.TestData;
using HE.Investments.AHP.ProjectDashboard.Domain.Tests.Project.TestDataBuilders;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Tests.Project.CommandHandlers;

public class TryCreateAhpProjectCommandHandlerTests : TestBase<TryCreateAhpProjectCommandHandler>
{
    [Fact]
    public async Task ShouldCreateAhpProject()
    {
        // given
        var prefillProject = ProjectPrefillDataTestData.FirstProjectPrefillData;

        var projectId = AhpProjectId.From(prefillProject.Id.Value);

        var userAccount = ConsortiumUserContextTestBuilder
            .New()
            .Register(this)
            .ConsortiumUserFromMock;

        var projectRepository = ProjectRepositoryTestBuilder
            .New()
            .CreateProject(
                prefillProject,
                userAccount,
                projectId)
            .BuildMockAndRegister(this);

        var prefillDataRepository = PrefillDataRepositoryTestBuilder
            .New()
            .ReturnProjectPrefillData(prefillProject.Id, userAccount, prefillProject)
            .BuildMockAndRegister(this);

        ProgrammeSettingsTestBuilder
            .New()
            .ReturnProgrammeId()
            .BuildMockAndRegister(this);

        ProgrammeTestBuilder
            .New()
            .ReturnAhpProgramme()
            .BuildMockAndRegister(this);

        // when
        var result = await TestCandidate.Handle(new TryCreateAhpProjectCommand(prefillProject.Id), CancellationToken.None);

        // then
        result.Should().Be(projectId);
        projectRepository.Verify(x => x.CreateProject(prefillProject, userAccount, CancellationToken.None), Times.Once);
        prefillDataRepository.Verify(x => x.GetProjectPrefillData(prefillProject.Id, userAccount, CancellationToken.None), Times.Once);
    }
}
