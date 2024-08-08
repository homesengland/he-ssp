using HE.Investments.Programme.Contract.Config;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investments.FrontDoor.Domain.Tests.Services.TestDataBuilders;

public class ProgrammeSettingsTestBuilder
{
    private readonly Mock<IProgrammeSettings> _mock;

    private ProgrammeSettingsTestBuilder()
    {
        _mock = new Mock<IProgrammeSettings>();
    }

    public static ProgrammeSettingsTestBuilder New() => new();

    public ProgrammeSettingsTestBuilder ReturnProgrammeId()
    {
        _mock.Setup(x => x.AhpProgrammeId).Returns(new ProgrammeBuilder().Build().Id.Value);
        return this;
    }

    public IProgrammeSettings Build()
    {
        return _mock.Object;
    }

    public Mock<IProgrammeSettings> BuildMockAndRegister(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return _mock;
    }
}
