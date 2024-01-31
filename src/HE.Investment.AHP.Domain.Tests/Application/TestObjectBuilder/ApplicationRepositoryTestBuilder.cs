using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.Repositories.Interfaces;
using HE.Investments.Account.Shared.User;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investment.AHP.Domain.Tests.Application.TestObjectBuilder;

public class ApplicationRepositoryTestBuilder
{
    private readonly Mock<IApplicationRepository> _mock;
    private readonly Mock<IApplicationWithdraw> _iApplicationWithdrawMock;
    private readonly Mock<IApplicationHold> _iApplicationHoldMock;
    private readonly Mock<IApplicationSubmit> _iApplicationSubmitMock;

    private ApplicationRepositoryTestBuilder()
    {
        _mock = new Mock<IApplicationRepository>();
        _iApplicationWithdrawMock = new Mock<IApplicationWithdraw>();
        _iApplicationHoldMock = new Mock<IApplicationHold>();
        _iApplicationSubmitMock = new Mock<IApplicationSubmit>();
    }

    public static ApplicationRepositoryTestBuilder New() => new();

    public ApplicationRepositoryTestBuilder ReturnApplication(AhpApplicationId applicationId, UserAccount userAccount, ApplicationEntity application)
    {
        _mock.Setup(x => x
                    .GetById(applicationId, userAccount, CancellationToken.None))
                .ReturnsAsync(application);

        return this;
    }

    public IApplicationRepository Build()
    {
        return _mock.Object;
    }

    public IApplicationWithdraw BuildIApplicationWithdraw()
    {
        return _iApplicationWithdrawMock.Object;
    }

    public IApplicationHold BuildIApplicationHold()
    {
        return _iApplicationHoldMock.Object;
    }

    public IApplicationSubmit BuildIApplicationSubmit()
    {
        return _iApplicationSubmitMock.Object;
    }

    public Mock<IApplicationRepository> BuildMockAndRegister(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return _mock;
    }

    public Mock<IApplicationWithdraw> BuildIApplicationWithdrawMockAndRegister(IRegisterDependency registerDependency)
    {
        var mockedObject = BuildIApplicationWithdraw();
        registerDependency.RegisterDependency(mockedObject);
        return _iApplicationWithdrawMock;
    }

    public Mock<IApplicationHold> BuildIApplicationHoldMockAndRegister(IRegisterDependency registerDependency)
    {
        var mockedObject = BuildIApplicationHold();
        registerDependency.RegisterDependency(mockedObject);
        return _iApplicationHoldMock;
    }

    public Mock<IApplicationSubmit> BuildIApplicationSubmitMockAndRegister(IRegisterDependency registerDependency)
    {
        var mockedObject = BuildIApplicationSubmit();
        registerDependency.RegisterDependency(mockedObject);
        return _iApplicationSubmitMock;
    }
}
