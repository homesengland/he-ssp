using HE.InvestmentLoans.BusinessLogic.Security;
using HE.InvestmentLoans.BusinessLogic.Security.Repositories;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using Moq;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Security.TestObjectBuilder;
internal class SecurityRepositoryTestBuilder
{
    private readonly Mock<ISecurityRepository> _mock;

    private SecurityRepositoryTestBuilder()
    {
        _mock = new Mock<ISecurityRepository>();
    }

    public static SecurityRepositoryTestBuilder New() => new();

    public SecurityRepositoryTestBuilder ReturnsSecurityEntity(
        SecurityEntity securityEntity)
    {
        _mock.Setup(x => x.GetAsync(It.IsAny<LoanApplicationId>(), It.IsAny<UserAccount>(), It.IsAny<CancellationToken>())).ReturnsAsync(securityEntity);
        return this;
    }

    public SecurityRepositoryTestBuilder ReturnsSecurityEntity(
        Action<SecurityEntityTestBuilder> buildAction)
    {
        var builder = SecurityEntityTestBuilder.New();

        buildAction(builder);

        _mock.Setup(x => x.GetAsync(It.IsAny<LoanApplicationId>(), It.IsAny<UserAccount>(), It.IsAny<CancellationToken>())).ReturnsAsync(builder.Build());

        return this;
    }

    public ISecurityRepository Build()
    {
        return _mock.Object;
    }

    public Mock<ISecurityRepository> BuildMockAndRegister(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return _mock;
    }
}
