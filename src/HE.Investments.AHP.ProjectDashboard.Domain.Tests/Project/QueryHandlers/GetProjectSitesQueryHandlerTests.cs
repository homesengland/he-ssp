using FluentAssertions;
using HE.Investment.AHP.Domain.Tests.Common.TestDataBuilders;
using HE.Investments.Account.Shared.User;
using HE.Investments.AHP.Consortium.Domain.Tests.TestData;
using HE.Investments.AHP.Consortium.Domain.Tests.TestObjectBuilders;
using HE.Investments.AHP.ProjectDashboard.Contract.Project.Queries;
using HE.Investments.AHP.ProjectDashboard.Domain.Project.QueryHandlers;
using HE.Investments.AHP.ProjectDashboard.Domain.Tests.Project.TestData;
using HE.Investments.AHP.ProjectDashboard.Domain.Tests.Project.TestDataBuilders;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Tests.Project.QueryHandlers;

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
        result.Sites.Items.Should().Contain(x => x.Id == projectSites.Sites![0].Id);
        result.Sites.Items.Should().Contain(x => x.Id == projectSites.Sites![1].Id);
    }

    [Fact]
    public async Task ShouldNotPrefillData_WhenUserCannotEditProject()
    {
        // given
        var projectSites = AhpProjectSitesTestData.AhpProjectWithNotCompletedSite;

        var userAccount = ConsortiumUserContextTestBuilder
            .New()
            .ReturnUserAccount(AhpUserAccountTestData.UserAccountOneWithConsortiumAsMember)
            .Register(this)
            .ConsortiumUserFromMock;

        var mock = PrefillDataRepositoryTestBuilder
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
        var result = await TestCandidate.Handle(new GetProjectSitesQuery(projectSites.Id, new PaginationRequest(1)), CancellationToken.None);

        // then
        result.ProjectId.Should().Be(projectSites.Id);
        result.ProjectName.Should().Be(projectSites.Name.Value);
        result.OrganisationName.Should().Be(userAccount.Organisation!.RegisteredCompanyName);
        result.Sites.Items.Should().NotBeEmpty();
        result.Sites.Items.Should().HaveCount(1);
        mock.Verify(x => x.GetProjectPrefillData(It.IsAny<FrontDoorProjectId>(), It.IsAny<UserAccount>(), It.IsAny<CancellationToken>()), Times.Never);
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
        result.Sites.Items.Should().Contain(x => x.Id == projectSites.Sites![0].Id);
    }
}
