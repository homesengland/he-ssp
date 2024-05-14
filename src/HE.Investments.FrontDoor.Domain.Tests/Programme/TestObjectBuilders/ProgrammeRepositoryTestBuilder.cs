using HE.Investments.FrontDoor.Domain.Programme.Repository;
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

    public ProgrammeRepositoryTestBuilder ReturnIsAnyAhpProgrammeAvailableResponse(DateOnly? expectedStartDate, bool response)
    {
        _mock.Setup(x => x
                    .IsAnyAhpProgrammeAvailable(expectedStartDate, CancellationToken.None))
                .ReturnsAsync(response);

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
