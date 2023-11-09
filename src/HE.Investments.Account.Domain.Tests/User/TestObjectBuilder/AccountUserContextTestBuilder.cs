using HE.Investments.Account.Domain.Tests.User.TestData;
using HE.Investments.Account.Domain.User;
using HE.Investments.Account.Shared.User;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investments.Account.Domain.Tests.User.TestObjectBuilder;

public class AccountUserContextTestBuilder
{
    private readonly Mock<IAccountUserContext> _mock;

    private AccountUserContextTestBuilder(UserAccount? userAccount, UserDetails? userDetails = null)
    {
        _mock = new Mock<IAccountUserContext>();
        ReturnUserAccount(userAccount ?? UserAccountTestData.UserAccountOne);
    }

    public UserAccount UserAccountFromMock { get; private set; }

    public static AccountUserContextTestBuilder New(UserAccount? userAccount = null) => new(userAccount);

    public AccountUserContextTestBuilder ReturnUserAccount(UserAccount userAccount)
    {
        UserAccountFromMock = userAccount;
        _mock.Setup(x => x.GetSelectedAccount()).ReturnsAsync(userAccount);
        _mock.Setup(x => x.UserGlobalId).Returns(userAccount.UserGlobalId);
        _mock.Setup(x => x.Email).Returns(userAccount.UserEmail);

        return this;
    }



    public AccountUserContextTestBuilder IsNotLinkedWithOrganization()
    {
        _mock.Setup(x => x.IsLinkedWithOrganization()).ReturnsAsync(false);

        return this;
    }

    public AccountUserContextTestBuilder IsLinkedWithOrganization()
    {
        _mock.Setup(x => x.IsLinkedWithOrganization()).ReturnsAsync(true);

        return this;
    }

    public IAccountUserContext Build()
    {
        return _mock.Object;
    }

    public AccountUserContextTestBuilder Register(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return this;
    }
}
