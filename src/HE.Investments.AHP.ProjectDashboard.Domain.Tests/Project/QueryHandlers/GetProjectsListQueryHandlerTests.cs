using FluentAssertions;
using HE.Investment.AHP.Domain.Tests.Common.TestDataBuilders;
using HE.Investments.AHP.Consortium.Domain.Tests.TestObjectBuilders;
using HE.Investments.AHP.ProjectDashboard.Contract.Project.Queries;
using HE.Investments.AHP.ProjectDashboard.Domain.Project.QueryHandlers;
using HE.Investments.AHP.ProjectDashboard.Domain.Project.ValueObjects;
using HE.Investments.AHP.ProjectDashboard.Domain.Tests.Project.TestData;
using HE.Investments.AHP.ProjectDashboard.Domain.Tests.Project.TestDataBuilders;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Tests.Project.QueryHandlers;

public class GetProjectsListQueryHandlerTests : TestBase<GetProjectsListQueryHandler>
{
    [Fact]
    public async Task ShouldReturnProjectsList()
    {
        // given
        var projectsWithSites = new PaginationResult<AhpProjectSites>(
            [
                AhpProjectSitesTestData.FirstAhpProjectWithSites,
                AhpProjectSitesTestData.SecondAhpProjectWithSites,
            ],
            1,
            10,
            2);

        var userAccount = ConsortiumUserContextTestBuilder
            .New()
            .Register(this)
            .ConsortiumUserFromMock;

        var paginationRequest = new PaginationRequest(1);

        ProjectRepositoryTestBuilder
            .New()
            .ReturnProjectsList(
                paginationRequest,
                userAccount,
                projectsWithSites)
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
        var result = await TestCandidate.Handle(new GetProjectsListQuery(paginationRequest), CancellationToken.None);

        // then
        result.Result.Items.Should().NotBeEmpty();
        result.Result.Items.Should().HaveCount(2);
        result.Result.Items.Should().Contain(x => x.ProjectId == AhpProjectSitesTestData.FirstAhpProjectWithSites.Id);
        result.Result.Items.Should().Contain(x => x.ProjectId == AhpProjectSitesTestData.SecondAhpProjectWithSites.Id);
    }

    [Fact]
    public async Task ShouldReturnEmptyProjectsList()
    {
        // given
        var projectsWithSites = new PaginationResult<AhpProjectSites>(
            [],
            1,
            10,
            0);

        var userAccount = ConsortiumUserContextTestBuilder
            .New()
            .Register(this)
            .ConsortiumUserFromMock;

        var paginationRequest = new PaginationRequest(1);

        ProjectRepositoryTestBuilder
            .New()
            .ReturnProjectsList(
                paginationRequest,
                userAccount,
                projectsWithSites)
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
        var result = await TestCandidate.Handle(new GetProjectsListQuery(paginationRequest), CancellationToken.None);

        // then
        result.Result.Items.Should().BeEmpty();
    }
}
