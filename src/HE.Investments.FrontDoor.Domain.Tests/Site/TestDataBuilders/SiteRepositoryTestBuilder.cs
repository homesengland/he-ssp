using HE.Investments.Account.Shared.User;
using HE.Investments.FrontDoor.Domain.Site;
using HE.Investments.FrontDoor.Domain.Site.Repository;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investments.FrontDoor.Domain.Tests.Site.TestDataBuilders;

public class SiteRepositoryTestBuilder
{
    private readonly Mock<ISiteRepository> _mock;
    private readonly Mock<IRemoveSiteRepository> _iremoveSiteRepository;

    private SiteRepositoryTestBuilder()
    {
        _mock = new Mock<ISiteRepository>();
        _iremoveSiteRepository = new Mock<IRemoveSiteRepository>();
    }

    public static SiteRepositoryTestBuilder New() => new();

    public SiteRepositoryTestBuilder ReturnProjectSites(FrontDoorProjectId projectId, UserAccount userAccount, ProjectSitesEntity projectSites)
    {
        _mock.Setup(x => x
                .GetProjectSites(projectId, userAccount, CancellationToken.None))
            .ReturnsAsync(projectSites);

        return this;
    }

    public ISiteRepository Build()
    {
        return _mock.Object;
    }

    public IRemoveSiteRepository BuildIRemoveSiteRepository()
    {
        return _iremoveSiteRepository.Object;
    }

    public Mock<ISiteRepository> BuildMockAndRegister(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return _mock;
    }

    public Mock<IRemoveSiteRepository> BuildIRemoveSiteRepositoryMockAndRegister(IRegisterDependency registerDependency)
    {
        var mockedObject = BuildIRemoveSiteRepository();
        registerDependency.RegisterDependency(mockedObject);
        return _iremoveSiteRepository;
    }
}
