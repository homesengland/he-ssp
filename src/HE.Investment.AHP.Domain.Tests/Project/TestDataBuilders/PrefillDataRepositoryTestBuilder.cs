using HE.Investment.AHP.Domain.UserContext;
using HE.Investments.AHP.Consortium.Shared.UserContext;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Data;
using HE.Investments.FrontDoor.Shared.Project.Repositories;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investment.AHP.Domain.Tests.Project.TestDataBuilders;

public class PrefillDataRepositoryTestBuilder
{
    private readonly Mock<IPrefillDataRepository> _mock;

    private PrefillDataRepositoryTestBuilder()
    {
        _mock = new Mock<IPrefillDataRepository>();
    }

    public static PrefillDataRepositoryTestBuilder New() => new();

    public PrefillDataRepositoryTestBuilder ReturnProjectPrefillData(
        FrontDoorProjectId fdProjectId,
        ConsortiumUserAccount userAccount,
        ProjectPrefillData projectPrefillData)
    {
        _mock.Setup(x => x.GetProjectPrefillData(fdProjectId, userAccount, CancellationToken.None)).ReturnsAsync(projectPrefillData);
        return this;
    }

    public IPrefillDataRepository Build()
    {
        return _mock.Object;
    }

    public Mock<IPrefillDataRepository> BuildMockAndRegister(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return _mock;
    }
}
