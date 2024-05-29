using HE.Investment.AHP.Domain.Tests.Project.TestData;
using HE.Investments.Programme.Contract.Queries;
using HE.Investments.TestsUtils.TestFramework;
using MediatR;
using Moq;

namespace HE.Investment.AHP.Domain.Tests.Common.TestDataBuilders;

public class ProgrammeTestBuilder
{
    private readonly Mock<IMediator> _mock;

    private ProgrammeTestBuilder()
    {
        _mock = new Mock<IMediator>();
    }

    public static ProgrammeTestBuilder New() => new();

    public ProgrammeTestBuilder ReturnAhpProgramme()
    {
        _mock.Setup(x => x.Send(new GetProgrammeQuery(ProgrammeTestData.AhpCmeProgramme.Id), CancellationToken.None))
            .ReturnsAsync(ProgrammeTestData.AhpCmeProgramme);
        return this;
    }

    public IMediator Build()
    {
        return _mock.Object;
    }

    public Mock<IMediator> BuildMockAndRegister(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return _mock;
    }
}
