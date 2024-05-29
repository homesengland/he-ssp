using FluentAssertions;
using HE.Investment.AHP.Contract.Project.Queries;
using HE.Investment.AHP.Domain.Project.QueryHandlers;
using HE.Investment.AHP.Domain.Tests.Common.TestDataBuilders;
using HE.Investment.AHP.Domain.Tests.Project.TestData;
using HE.Investment.AHP.Domain.Tests.Project.TestDataBuilders;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.Project.QueryHandlers;

public class GetProjectDetailsQueryHandlerTests : TestBase<GetProjectDetailsQueryHandler>
{
    [Fact]
    public async Task ShouldReturnProjectDetailsWithApplications()
    {
        // given
        var projectDetails = AhpProjectApplicationsTestData.ThirdAhpProjectWithApplications;
        var paginationRequest = new PaginationRequest(1);

        var userAccount = AhpUserContextTestBuilder
            .New()
            .Register(this)
            .AhpUserFromMock;

        ProjectRepositoryTestBuilder
            .New()
            .ReturnProjectApplications(
                projectDetails.Id,
                userAccount,
                projectDetails)
            .BuildMockAndRegister(this);

        ProgrammeTestBuilder
            .New()
            .ReturnAhpProgramme()
            .WithProgrammeSettings()
            .BuildMockAndRegister(this);

        // when
        var result = await TestCandidate.Handle(new GetProjectDetailsQuery(projectDetails.Id, paginationRequest), CancellationToken.None);

        // then
        result.ProjectId.Should().Be(projectDetails.Id);
        result.ProjectName.Should().Be(projectDetails.Name.Value);
        result.OrganisationName.Should().Be(userAccount.Organisation!.RegisteredCompanyName);
        result.Applications.Items.Should().NotBeEmpty();
        result.Applications.Items.Should().HaveCount(3);
        result.Applications.Items.Should().Contain(x => x.Id == projectDetails.Applications[0].Id);
        result.Applications.Items.Should().Contain(x => x.Id == projectDetails.Applications[1].Id);
        result.Applications.Items.Should().Contain(x => x.Id == projectDetails.Applications[2].Id);
    }

    [Fact]
    public async Task ShouldReturnProjectDetailsWithoutApplications()
    {
        // given
        var projectDetails = AhpProjectApplicationsTestData.AhpProjectWithoutApplications;
        var paginationRequest = new PaginationRequest(1);

        var userAccount = AhpUserContextTestBuilder
            .New()
            .Register(this)
            .AhpUserFromMock;

        ProjectRepositoryTestBuilder
            .New()
            .ReturnProjectApplications(
                projectDetails.Id,
                userAccount,
                projectDetails)
            .BuildMockAndRegister(this);

        ProgrammeTestBuilder
            .New()
            .ReturnAhpProgramme()
            .WithProgrammeSettings()
            .BuildMockAndRegister(this);

        // when
        var result = await TestCandidate.Handle(new GetProjectDetailsQuery(projectDetails.Id, paginationRequest), CancellationToken.None);

        // then
        result.ProjectId.Should().Be(projectDetails.Id);
        result.ProjectName.Should().Be(projectDetails.Name.Value);
        result.OrganisationName.Should().Be(userAccount.Organisation!.RegisteredCompanyName);
        result.Applications.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldReturnProjectDetailsWithPagination()
    {
        // given
        var projectDetails = AhpProjectApplicationsTestData.FirstAhpProjectWithApplications;
        var paginationRequest = new PaginationRequest(1, 1);

        var userAccount = AhpUserContextTestBuilder
            .New()
            .Register(this)
            .AhpUserFromMock;

        ProjectRepositoryTestBuilder
            .New()
            .ReturnProjectApplications(
                projectDetails.Id,
                userAccount,
                projectDetails)
            .BuildMockAndRegister(this);

        ProgrammeTestBuilder
            .New()
            .ReturnAhpProgramme()
            .WithProgrammeSettings()
            .BuildMockAndRegister(this);

        // when
        var result = await TestCandidate.Handle(new GetProjectDetailsQuery(projectDetails.Id, paginationRequest), CancellationToken.None);

        // then
        result.ProjectId.Should().Be(projectDetails.Id);
        result.ProjectName.Should().Be(projectDetails.Name.Value);
        result.OrganisationName.Should().Be(userAccount.Organisation!.RegisteredCompanyName);
        result.Applications.Items.Should().NotBeEmpty();
        result.Applications.Items.Should().HaveCount(1);
        result.Applications.Items.Should().Contain(x => x.Id == projectDetails.Applications[0].Id);
    }
}
