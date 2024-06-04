using FluentAssertions;
using HE.Investment.AHP.Domain.Project.Repositories;
using HE.Investment.AHP.Domain.Tests.Common.TestDataBuilders;
using HE.Investment.AHP.Domain.Tests.Project.TestData;
using HE.Investment.AHP.Domain.Tests.Project.TestDataBuilders;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.Project.Repositories;

public class GetProjectSitesTests : TestBase<ProjectRepository>
{
    [Fact]
    public async Task ShouldReturnProjectSites()
    {
        // given
        var ahpProjectDto = AhpProjectDtoTestData.FirstAhpProject;

        var userAccount = AhpUserContextTestBuilder
            .New()
            .Register(this)
            .ConsortiumUserFromMock;

        ProjectCrmContextTestBuilder
            .New()
            .ReturnProject(ahpProjectDto.AhpProjectId, ahpProjectDto)
            .BuildMockAndRegister(this);

        // when
        var result = await TestCandidate.GetProjectSites(
            FrontDoorProjectId.From(ahpProjectDto.AhpProjectId), userAccount, CancellationToken.None);

        // then
        result.Sites.Should().NotBeEmpty();
        result.Sites.Should().HaveCount(2);
        result.Sites.Should().Contain(x => x.Id.ToGuidAsString() == SiteDtoTestData.FirstSite.id);
        result.Sites.Should().Contain(x => x.Id.ToGuidAsString() == SiteDtoTestData.SecondSite.id);
    }

    [Fact]
    public async Task ShouldReturnEmptyProjectSites()
    {
        // given
        var ahpProjectDto = AhpProjectDtoTestData.FourthAhpProjectWithoutSitesAndApplications;

        var userAccount = AhpUserContextTestBuilder
            .New()
            .Register(this)
            .ConsortiumUserFromMock;

        ProjectCrmContextTestBuilder
            .New()
            .ReturnProject(ahpProjectDto.AhpProjectId, ahpProjectDto)
            .BuildMockAndRegister(this);

        // when
        var result = await TestCandidate.GetProjectSites(
            FrontDoorProjectId.From(ahpProjectDto.AhpProjectId), userAccount, CancellationToken.None);

        // then
        result.Sites.Should().BeEmpty();
    }
}
