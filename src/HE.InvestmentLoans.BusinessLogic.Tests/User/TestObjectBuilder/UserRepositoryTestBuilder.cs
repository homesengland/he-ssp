extern alias Org;

using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.BusinessLogic.User.Repositories;
using HE.InvestmentLoans.Contract.User.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
public class UserRepositoryTestBuilder
{
    private readonly Mock<ILoanUserRepository> _mock;

    private UserRepositoryTestBuilder()
    {
        _mock = new Mock<ILoanUserRepository>();
    }

    public static UserRepositoryTestBuilder New() => new();

    public UserRepositoryTestBuilder ReturnUserDetailsEntity(
        UserGlobalId userGlobalId,
        UserDetails userDetails)
    {
        _mock.Setup(x => x.GetUserDetails(userGlobalId)).ReturnsAsync(userDetails);
        return this;
    }

    public ILoanUserRepository Build()
    {
        return _mock.Object;
    }

    public Mock<ILoanUserRepository> BuildMockAndRegister(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return _mock;
    }
}
