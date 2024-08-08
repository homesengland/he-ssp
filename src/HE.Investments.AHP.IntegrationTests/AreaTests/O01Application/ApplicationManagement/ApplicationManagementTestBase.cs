using FluentAssertions;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Application.Crm;
using HE.Investment.AHP.Domain.Site.Crm;
using HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.ApplicationManagement.Data.Model;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.ProjectDashboard.Domain.Project.Crm;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.ApplicationManagement;

public class ApplicationManagementTestBase : AhpIntegrationTest
{
    private readonly PagingRequestDto _pagingRequestDto = new() { pageNumber = 1, pageSize = 100 };

    private readonly IProjectCrmContext _projectCrmContext;

    private readonly ISiteCrmContext _siteCrmContext;

    private readonly IApplicationCrmContext _applicationCrmContext;

    protected ApplicationManagementTestBase(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
        _projectCrmContext = fixture.ServiceProvider.GetRequiredService<IProjectCrmContext>();
        _siteCrmContext = fixture.ServiceProvider.GetRequiredService<ISiteCrmContext>();
        _applicationCrmContext = fixture.ServiceProvider.GetRequiredService<IApplicationCrmContext>();
        AssertionOptions.FormattingOptions.MaxDepth = 10;
    }

    protected async Task TestGetProjectsEndpoint(UserData userData, AhpProject[] expectedAhpProjects)
    {
        // given
        var (_, userGlobalId, organisationId, consortiumId) = userData;

        // when
        var requestResult = await _projectCrmContext.GetProjects(userGlobalId, organisationId, consortiumId, _pagingRequestDto, CancellationToken.None);
        var ahpProjectsFromEndpoint = ConvertToAhpProjects(requestResult.items);

        // then
        var expectedAhpProjectsWithoutApplications = expectedAhpProjects.Select(x => x with { Sites = x.Sites.Select(y => y with { Applications = [] }).ToArray() }).ToArray();
        ahpProjectsFromEndpoint.Should().BeEquivalentTo(expectedAhpProjectsWithoutApplications);
    }

    protected async Task TestGetProjectEndpoint(UserData userData, AhpProject expectedAhpProject)
    {
        // given
        var (_, userGlobalId, organisationId, consortiumId) = userData;

        // when
        var projectDto = await _projectCrmContext.GetProject(expectedAhpProject.ProjectId, userGlobalId, organisationId, consortiumId, CancellationToken.None);
        var ahpProject = ConvertToAhpProject(projectDto);

        // then
        ahpProject.Should().BeEquivalentTo(expectedAhpProject);
    }

    protected async Task TestGetSiteApplicationsEndpoint(UserData userData, AhpSite expectedAhpSiteApplications)
    {
        // given
        var (_, userGlobalId, organisationId, consortiumId) = userData;

        // when
        var siteApplicationsDto = await _siteCrmContext.GetSiteApplications(expectedAhpSiteApplications.SiteId, organisationId, userGlobalId, consortiumId, CancellationToken.None);
        var ahpSiteApplications = ConvertToAhpSites(siteApplicationsDto);

        // then
        ahpSiteApplications.Should().BeEquivalentTo(expectedAhpSiteApplications);
    }

    protected async Task TestGetApplicationEndpoint(UserData userData, AhpApplication expectedAhpApplication)
    {
        // given
        var (_, _, organisationId, _) = userData;

        // when
        var applicationDto = await _applicationCrmContext.GetOrganisationApplicationById(expectedAhpApplication.ApplicationId, organisationId, CancellationToken.None);
        var ahpApplication = ConvertToAhpApplication(applicationDto);

        // then
        ahpApplication.Should().BeEquivalentTo(expectedAhpApplication);
    }

    private AhpProject[] ConvertToAhpProjects(IList<AhpProjectDto> ahpProjectsDto)
    {
        return ahpProjectsDto.Select(projectDto =>
                new AhpProject(
                    projectDto.FrontDoorProjectId,
                    projectDto.AhpProjectName,
                    (projectDto.ListOfSites ?? []).Select(siteDto => new AhpSite(
                        siteDto.id,
                        siteDto.name,
                        (projectDto.ListOfApplications ?? [])
                        .Where(z => z.siteId == siteDto.id)
                        .Select(applicationDto => new AhpApplication(applicationDto.id, applicationDto.name))
                        .ToArray()))
                    .ToArray()))
            .ToArray();
    }

    private AhpProject ConvertToAhpProject(AhpProjectDto ahpProjectsDto)
    {
        return new AhpProject(
            ahpProjectsDto.FrontDoorProjectId,
            ahpProjectsDto.AhpProjectName,
            (ahpProjectsDto.ListOfSites ?? []).Select(siteDto => new AhpSite(
                siteDto.id,
                siteDto.name,
                (ahpProjectsDto.ListOfApplications ?? [])
                .Select(applicationDto => new AhpApplication(applicationDto.id, applicationDto.name))
                .ToArray()))
            .ToArray());
    }

    private AhpSite ConvertToAhpSites(AhpSiteApplicationDto siteApplicationsDto)
    {
        return new AhpSite(
            siteApplicationsDto.SiteId,
            siteApplicationsDto.siteName,
            siteApplicationsDto.AhpApplications.Select(x => new AhpApplication(x.applicationId, x.applicationName))
                .ToArray());
    }

    private AhpApplication ConvertToAhpApplication(AhpApplicationDto applicationDto)
    {
        return new AhpApplication(applicationDto.id, applicationDto.name);
    }
}
