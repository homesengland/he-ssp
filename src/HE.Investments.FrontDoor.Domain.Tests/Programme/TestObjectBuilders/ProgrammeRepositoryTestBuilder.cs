using HE.Investments.FrontDoor.Domain.Programme;
using HE.Investments.FrontDoor.Domain.Programme.Repository;
using HE.Investments.FrontDoor.Domain.Tests.Programme.TestData;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investments.FrontDoor.Domain.Tests.Programme.TestObjectBuilders;

public class ProgrammeRepositoryTestBuilder
{
    private readonly Mock<IProgrammeRepository> _mock;

    private ProgrammeRepositoryTestBuilder()
    {
        _mock = new Mock<IProgrammeRepository>();
    }

    public static ProgrammeRepositoryTestBuilder New() => new();

    public ProgrammeRepositoryTestBuilder ReturnProgrammes()
    {
        _mock.Setup(x => x
                    .GetProgrammes(CancellationToken.None))
                .ReturnsAsync([ProgrammeTestData.ImportantProgramme]);

        return this;
    }

    public IProgrammeRepository Build()
    {
        return _mock.Object;
    }

    public Mock<IProgrammeRepository> BuildMockAndRegister(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return _mock;
    }
}
