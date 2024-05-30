using HE.Investments.FrontDoor.Domain.Services;
using HE.Investments.Programme.Contract;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investments.FrontDoor.Domain.Tests.Services.TestDataBuilders;

public class ProgrammeAvailabilityServiceTestBuilder
{
    private readonly Mock<IProgrammeAvailabilityService> _mock;

    private ProgrammeAvailabilityServiceTestBuilder()
    {
        _mock = new Mock<IProgrammeAvailabilityService>();
    }

    public static ProgrammeAvailabilityServiceTestBuilder New() => new();

    public ProgrammeAvailabilityServiceTestBuilder ReturnIsStartDateValidForProgramme()
    {
        _mock.Setup(x =>
                x.IsStartDateValidForProgramme(It.IsAny<ProgrammeId>(), It.IsAny<DateOnly>(), CancellationToken.None))
            .ReturnsAsync(true);
        return this;
    }

    public IProgrammeAvailabilityService Build()
    {
        return _mock.Object;
    }

    public Mock<IProgrammeAvailabilityService> BuildMockAndRegister(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return _mock;
    }
}
