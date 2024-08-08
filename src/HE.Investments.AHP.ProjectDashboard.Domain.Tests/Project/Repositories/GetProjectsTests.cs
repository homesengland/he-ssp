using FluentAssertions;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.AHP.Consortium.Domain.Tests.TestObjectBuilders;
using HE.Investments.AHP.ProjectDashboard.Domain.Project.Repositories;
using HE.Investments.AHP.ProjectDashboard.Domain.Tests.Project.TestData;
using HE.Investments.AHP.ProjectDashboard.Domain.Tests.Project.TestDataBuilders;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Tests.Project.Repositories;

public class GetProjectsTests : TestBase<ProjectRepository>
{
    [Fact]
    public async Task ShouldReturnProjects()
    {
        // given
        var projectsListDto = new PagedResponseDto<AhpProjectDto>
        {
            items =
            [
                AhpProjectDtoTestData.FirstAhpProject,
                AhpProjectDtoTestData.SecondAhpProject,
                AhpProjectDtoTestData.ThirdAhpProject,
                AhpProjectDtoTestData.FourthAhpProjectWithoutSitesAndApplications,
            ],
            totalItemsCount = 4,
        };

        var userAccount = ConsortiumUserContextTestBuilder
            .New()
            .Register(this)
            .ConsortiumUserFromMock;

        var paginationRequest = new PaginationRequest(1);

        ProjectCrmContextTestBuilder
            .New()
            .ReturnProjects(projectsListDto)
            .BuildMockAndRegister(this);

        // when
        var result = await TestCandidate.GetProjects(paginationRequest, userAccount, CancellationToken.None);

        // then
        result.Items.Should().NotBeEmpty();
        result.Items.Should().HaveCount(4);
        result.Items.Should().Contain(x => x.Id.Value == AhpProjectDtoTestData.FirstAhpProject.FrontDoorProjectId);
        result.Items.Should().Contain(x => x.Id.Value == AhpProjectDtoTestData.SecondAhpProject.FrontDoorProjectId);
        result.Items.Should().Contain(x => x.Id.Value == AhpProjectDtoTestData.ThirdAhpProject.FrontDoorProjectId);
        result.Items.Should().Contain(x => x.Id.Value == AhpProjectDtoTestData.FourthAhpProjectWithoutSitesAndApplications.FrontDoorProjectId);
    }

    [Fact]
    public async Task ShouldReturnEmptyProjectsList()
    {
        // given
        var projectsListDto = new PagedResponseDto<AhpProjectDto>
        {
            items = Array.Empty<AhpProjectDto>(),
            totalItemsCount = 0,
        };

        var userAccount = ConsortiumUserContextTestBuilder
            .New()
            .Register(this)
            .ConsortiumUserFromMock;

        var paginationRequest = new PaginationRequest(1);

        ProjectCrmContextTestBuilder
            .New()
            .ReturnProjects(projectsListDto)
            .BuildMockAndRegister(this);

        // when
        var result = await TestCandidate.GetProjects(paginationRequest, userAccount, CancellationToken.None);

        // then
        result.Items.Should().BeEmpty();
    }
}
