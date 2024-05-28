using HE.Investment.AHP.Domain.Config;
using HE.Investment.AHP.Domain.Tests.Project.TestData;
using HE.Investments.Programme.Contract.Queries;
using HE.Investments.TestsUtils.TestFramework;
using MediatR;
using Moq;

namespace HE.Investment.AHP.Domain.Tests.Common.TestDataBuilders;

public class ProgrammeTestBuilder
{
    private readonly Mock<IMediator> _mediatorMock;

    private readonly Mock<IProgrammeSettings> _programmeSettingsMock;

    private ProgrammeTestBuilder()
    {
        _mediatorMock = new Mock<IMediator>();
        _programmeSettingsMock = new Mock<IProgrammeSettings>();
    }

    public static ProgrammeTestBuilder New() => new();

    public ProgrammeTestBuilder ReturnAhpProgramme()
    {
        _mediatorMock.Setup(x => x.Send(new GetProgrammeQuery(ProgrammeTestData.AhpCmeProgramme.Id), CancellationToken.None))
            .ReturnsAsync(ProgrammeTestData.AhpCmeProgramme);
        return this;
    }

    public ProgrammeTestBuilder WithProgrammeSettings()
    {
        _programmeSettingsMock.Setup(x => x.AhpProgrammeId).Returns(ProgrammeTestData.AhpCmeProgramme.Id.Value);
        return this;
    }

    public IMediator BuildMediatorMock()
    {
        return _mediatorMock.Object;
    }

    public IProgrammeSettings BuildProgrammeSettingsMock()
    {
        return _programmeSettingsMock.Object;
    }

    public Mock<IMediator> BuildMockAndRegister(IRegisterDependency registerDependency)
    {
        var mediatorMockedObject = BuildMediatorMock();
        var programmeSettingsObject = BuildProgrammeSettingsMock();
        registerDependency.RegisterDependency(mediatorMockedObject);
        registerDependency.RegisterDependency(programmeSettingsObject);
        return _mediatorMock;
    }
}
