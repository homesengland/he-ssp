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
    private readonly Mock<IChangeApplicationStatus> _iChangeApplicationStatus;

    private ApplicationRepositoryTestBuilder()
    {
        _mock = new Mock<IApplicationRepository>();
        _iChangeApplicationStatus = new Mock<IChangeApplicationStatus>();
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

    public IChangeApplicationStatus BuildIChangeApplicationStatus()
    {
        return _iChangeApplicationStatus.Object;
    }

    public Mock<IApplicationRepository> BuildMockAndRegister(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return _mock;
    }

    public Mock<IChangeApplicationStatus> BuildIChangeApplicationStatusMockAndRegister(IRegisterDependency registerDependency)
    {
        var mockedObject = BuildIChangeApplicationStatus();
        registerDependency.RegisterDependency(mockedObject);
        return _iChangeApplicationStatus;
    }
}
