using HE.Investment.AHP.Domain.Config;
using HE.Investment.AHP.Domain.Tests.Project.TestData;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investment.AHP.Domain.Tests.Common.TestDataBuilders;

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
        _mock.Setup(x => x.AhpProgrammeId).Returns(ProgrammeTestData.AhpCmeProgramme.Id.Value);
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
