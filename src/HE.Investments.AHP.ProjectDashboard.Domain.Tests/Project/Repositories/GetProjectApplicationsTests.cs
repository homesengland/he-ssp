using FluentAssertions;
using HE.Investments.AHP.Consortium.Domain.Tests.TestObjectBuilders;
using HE.Investments.AHP.ProjectDashboard.Domain.Project.Repositories;
using HE.Investments.AHP.ProjectDashboard.Domain.Tests.Project.TestData;
using HE.Investments.AHP.ProjectDashboard.Domain.Tests.Project.TestDataBuilders;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Tests.Project.Repositories;

public class GetProjectApplicationsTests : TestBase<ProjectRepository>
{
    [Fact]
    public async Task ShouldReturnProjectApplications()
    {
        // given
        var ahpProjectDto = AhpProjectDtoTestData.FirstAhpProject;

        var userAccount = ConsortiumUserContextTestBuilder
            .New()
            .Register(this)
            .ConsortiumUserFromMock;

        ProjectCrmContextTestBuilder
            .New()
            .ReturnProject(ahpProjectDto.AhpProjectId, ahpProjectDto)
            .BuildMockAndRegister(this);

        // when
        var result = await TestCandidate.GetProjectOverview(
            FrontDoorProjectId.From(ahpProjectDto.AhpProjectId),
            userAccount,
            CancellationToken.None);

        // then
        result.Applications.Should().NotBeEmpty();
        result.Applications.Should().HaveCount(2);
        result.Applications.Should().Contain(x => x.Id.Value == AhpApplicationDtoTestData.FirstAhpApplication.id);
        result.Applications.Should().Contain(x => x.Id.Value == AhpApplicationDtoTestData.SecondAhpApplication.id);
    }

    [Fact]
    public async Task ShouldReturnEmptyProjectApplications()
    {
        // given
        var ahpProjectDto = AhpProjectDtoTestData.FourthAhpProjectWithoutSitesAndApplications;

        var userAccount = ConsortiumUserContextTestBuilder
            .New()
            .Register(this)
            .ConsortiumUserFromMock;

        ProjectCrmContextTestBuilder
            .New()
            .ReturnProject(ahpProjectDto.AhpProjectId, ahpProjectDto)
            .BuildMockAndRegister(this);

        // when
        var result = await TestCandidate.GetProjectOverview(
            FrontDoorProjectId.From(ahpProjectDto.AhpProjectId),
            userAccount,
            CancellationToken.None);

        // then
        result.Applications.Should().BeEmpty();
    }
}
