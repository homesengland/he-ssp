using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Project.Crm;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investment.AHP.Domain.Tests.Project.TestDataBuilders;

public class ProjectCrmContextTestBuilder
{
    private readonly Mock<IProjectCrmContext> _mock;

    private ProjectCrmContextTestBuilder()
    {
        _mock = new Mock<IProjectCrmContext>();
    }

    public static ProjectCrmContextTestBuilder New() => new();

    public ProjectCrmContextTestBuilder ReturnProject(
        string projectId,
        AhpProjectDto ahpProjectDto)
    {
        _mock.Setup(x =>
                x.GetProject(
                    projectId,
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string?>(),
                    CancellationToken.None))
            .ReturnsAsync(ahpProjectDto);
        return this;
    }

    public ProjectCrmContextTestBuilder ReturnProjects(PagedResponseDto<AhpProjectDto> ahpProjectsList)
    {
        _mock.Setup(x =>
                x.GetProjects(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string?>(),
                    It.IsAny<PagingRequestDto>(),
                    CancellationToken.None))
            .ReturnsAsync(ahpProjectsList);
        return this;
    }

    public ProjectCrmContextTestBuilder CreateProject(
        string frontDoorProjectId,
        string projectName,
        string ahpProjectId)
    {
        _mock.Setup(x =>
                x.CreateProject(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string?>(),
                    frontDoorProjectId,
                    projectName,
                    It.IsAny<IList<SiteDto>>(),
                    CancellationToken.None))
            .ReturnsAsync(ahpProjectId);
        return this;
    }

    public IProjectCrmContext Build()
    {
        return _mock.Object;
    }

    public Mock<IProjectCrmContext> BuildMockAndRegister(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return _mock;
    }
}
