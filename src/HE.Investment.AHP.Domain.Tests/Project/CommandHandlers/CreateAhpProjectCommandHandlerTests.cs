using FluentAssertions;
using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Contract.Project.Commands;
using HE.Investment.AHP.Domain.Project.CommandHandlers;
using HE.Investment.AHP.Domain.Tests.Common.TestDataBuilders;
using HE.Investment.AHP.Domain.Tests.Project.TestData;
using HE.Investment.AHP.Domain.Tests.Project.TestDataBuilders;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investment.AHP.Domain.Tests.Project.CommandHandlers;

public class CreateAhpProjectCommandHandlerTests : TestBase<CreateAhpProjectCommandHandler>
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
        var result = await TestCandidate.Handle(new CreateAhpProjectCommand(prefillProject.Id), CancellationToken.None);

        // then
        result.Should().Be(projectId);
        projectRepository.Verify(x => x.CreateProject(prefillProject, userAccount, CancellationToken.None), Times.Once);
        prefillDataRepository.Verify(x => x.GetProjectPrefillData(prefillProject.Id, userAccount, CancellationToken.None), Times.Once);
    }
}
