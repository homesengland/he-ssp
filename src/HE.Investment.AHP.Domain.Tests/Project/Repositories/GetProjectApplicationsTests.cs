using FluentAssertions;
using HE.Investment.AHP.Domain.Project.Repositories;
using HE.Investment.AHP.Domain.Tests.Common.TestDataBuilders;
using HE.Investment.AHP.Domain.Tests.Project.TestData;
using HE.Investment.AHP.Domain.Tests.Project.TestDataBuilders;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.Project.Repositories;

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
        var result = await TestCandidate.GetProjectApplications(
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
        var result = await TestCandidate.GetProjectApplications(
            FrontDoorProjectId.From(ahpProjectDto.AhpProjectId),
            userAccount,
            CancellationToken.None);

        // then
        result.Applications.Should().BeEmpty();
    }
}
