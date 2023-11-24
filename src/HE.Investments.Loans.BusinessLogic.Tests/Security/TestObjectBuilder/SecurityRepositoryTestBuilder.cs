using HE.Investments.Account.Shared.User;
using HE.Investments.Loans.BusinessLogic.Security;
using HE.Investments.Loans.BusinessLogic.Security.Repositories;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investments.Loans.BusinessLogic.Tests.Security.TestObjectBuilder;
internal sealed class SecurityRepositoryTestBuilder
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
        _mock.Setup(x => x.GetAsync(It.IsAny<LoanApplicationId>(), It.IsAny<UserAccount>(), It.IsAny<SecurityFieldsSet>(), It.IsAny<CancellationToken>())).ReturnsAsync(securityEntity);
        return this;
    }

    public SecurityRepositoryTestBuilder ReturnsSecurityEntity(
        Action<SecurityEntityTestBuilder> buildAction)
    {
        var builder = SecurityEntityTestBuilder.New();

        buildAction(builder);

        _mock.Setup(x => x.GetAsync(It.IsAny<LoanApplicationId>(), It.IsAny<UserAccount>(), It.IsAny<SecurityFieldsSet>(), It.IsAny<CancellationToken>())).ReturnsAsync(builder.Build());

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
