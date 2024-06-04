using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Domain.Project.Repositories;
using HE.Investment.AHP.Domain.Project.ValueObjects;
using HE.Investment.AHP.Domain.UserContext;
using HE.Investments.AHP.Consortium.Shared.UserContext;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Data;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investment.AHP.Domain.Tests.Project.TestDataBuilders;

public class ProjectRepositoryTestBuilder
{
    private readonly Mock<IProjectRepository> _mock;

    private ProjectRepositoryTestBuilder()
    {
        _mock = new Mock<IProjectRepository>();
    }

    public static ProjectRepositoryTestBuilder New() => new();

    public ProjectRepositoryTestBuilder ReturnProjectApplications(
        FrontDoorProjectId fdProjectId,
        ConsortiumUserAccount userAccount,
        AhpProjectApplications ahpProjectApplications)
    {
        _mock.Setup(x => x.GetProjectApplications(fdProjectId, userAccount, CancellationToken.None)).ReturnsAsync(ahpProjectApplications);
        return this;
    }

    public ProjectRepositoryTestBuilder ReturnProjectSites(
        FrontDoorProjectId fdProjectId,
        ConsortiumUserAccount userAccount,
        AhpProjectSites ahpProjectSites)
    {
        _mock.Setup(x => x.GetProjectSites(fdProjectId, userAccount, CancellationToken.None)).ReturnsAsync(ahpProjectSites);
        return this;
    }

    public ProjectRepositoryTestBuilder ReturnProjectsList(
        PaginationRequest pagination,
        ConsortiumUserAccount userAccount,
        PaginationResult<AhpProjectSites> ahpProjectSites)
    {
        _mock.Setup(x => x.GetProjects(pagination, userAccount, CancellationToken.None)).ReturnsAsync(ahpProjectSites);
        return this;
    }

    public ProjectRepositoryTestBuilder CreateProject(
        ProjectPrefillData projectPrefillData,
        ConsortiumUserAccount userAccount,
        AhpProjectId ahpProjectId)
    {
        _mock.Setup(x => x.CreateProject(projectPrefillData, userAccount, CancellationToken.None)).ReturnsAsync(ahpProjectId);
        return this;
    }

    public IProjectRepository Build()
    {
        return _mock.Object;
    }

    public Mock<IProjectRepository> BuildMockAndRegister(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return _mock;
    }
}
