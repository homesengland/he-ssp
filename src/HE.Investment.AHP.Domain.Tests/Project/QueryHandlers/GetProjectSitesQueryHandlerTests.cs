using FluentAssertions;
using HE.Investment.AHP.Contract.Project.Queries;
using HE.Investment.AHP.Domain.Project.QueryHandlers;
using HE.Investment.AHP.Domain.Tests.Common.TestDataBuilders;
using HE.Investment.AHP.Domain.Tests.Project.TestData;
using HE.Investment.AHP.Domain.Tests.Project.TestDataBuilders;
using HE.Investments.AHP.Consortium.Domain.Tests.TestObjectBuilders;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Tests.TestData;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.Project.QueryHandlers;

public class GetProjectSitesQueryHandlerTests : TestBase<GetProjectSitesQueryHandler>
{
    [Fact]
    public async Task ShouldReturnProjectSites()
    {
        // given
        var projectSites = AhpProjectSitesTestData.FirstAhpProjectWithSites;
        var paginationRequest = new PaginationRequest(1);

        var userAccount = ConsortiumUserContextTestBuilder
            .New()
            .Register(this)
            .ConsortiumUserFromMock;

        PrefillDataRepositoryTestBuilder
            .New()
            .ReturnProjectPrefillData(projectSites.Id, userAccount, ProjectPrefillDataTestData.FirstProjectPrefillData)
            .BuildMockAndRegister(this);

        ProjectRepositoryTestBuilder
            .New()
            .ReturnProjectSites(
                projectSites.Id,
                userAccount,
                projectSites)
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
        var result = await TestCandidate.Handle(new GetProjectSitesQuery(projectSites.Id, paginationRequest), CancellationToken.None);

        // then
        result.ProjectId.Should().Be(projectSites.Id);
        result.ProjectName.Should().Be(projectSites.Name.Value);
        result.OrganisationName.Should().Be(userAccount.Organisation!.RegisteredCompanyName);
        result.Sites.Items.Should().NotBeEmpty();
        result.Sites.Items.Should().HaveCount(2);
        result.Sites.Items.Should().Contain(x => x.Id == projectSites.Sites![0].Id.ToString());
        result.Sites.Items.Should().Contain(x => x.Id == projectSites.Sites![1].Id.ToString());
    }

    [Fact]
    public async Task ShouldReturnEmptyProjectSites()
    {
        // given
        var projectSites = AhpProjectSitesTestData.ProjectWithoutSites;
        var paginationRequest = new PaginationRequest(1);

        var userAccount = ConsortiumUserContextTestBuilder
            .New()
            .Register(this)
            .ConsortiumUserFromMock;

        PrefillDataRepositoryTestBuilder
            .New()
            .ReturnProjectPrefillData(projectSites.Id, userAccount, ProjectPrefillDataTestData.FirstProjectPrefillData)
            .BuildMockAndRegister(this);

        ProjectRepositoryTestBuilder
            .New()
            .ReturnProjectSites(
                projectSites.Id,
                userAccount,
                projectSites)
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
        var result = await TestCandidate.Handle(new GetProjectSitesQuery(projectSites.Id, paginationRequest), CancellationToken.None);

        // then
        result.ProjectId.Should().Be(projectSites.Id);
        result.ProjectName.Should().Be(projectSites.Name.Value);
        result.OrganisationName.Should().Be(userAccount.Organisation!.RegisteredCompanyName);
        result.Sites.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldReturnProjectSitesWithPagination()
    {
        // given
        var projectSites = AhpProjectSitesTestData.FirstAhpProjectWithSites;
        var paginationRequest = new PaginationRequest(1, 1);

        var userAccount = ConsortiumUserContextTestBuilder
            .New()
            .Register(this)
            .ConsortiumUserFromMock;

        PrefillDataRepositoryTestBuilder
            .New()
            .ReturnProjectPrefillData(projectSites.Id, userAccount, ProjectPrefillDataTestData.FirstProjectPrefillData)
            .BuildMockAndRegister(this);

        ProjectRepositoryTestBuilder
            .New()
            .ReturnProjectSites(
                projectSites.Id,
                userAccount,
                projectSites)
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
        var result = await TestCandidate.Handle(new GetProjectSitesQuery(projectSites.Id, paginationRequest), CancellationToken.None);

        // then
        result.ProjectId.Should().Be(projectSites.Id);
        result.ProjectName.Should().Be(projectSites.Name.Value);
        result.OrganisationName.Should().Be(userAccount.Organisation!.RegisteredCompanyName);
        result.Sites.Items.Should().NotBeEmpty();
        result.Sites.Items.Should().HaveCount(1);
        result.Sites.Items.Should().Contain(x => x.Id == projectSites.Sites![0].Id.ToString());
    }
}
