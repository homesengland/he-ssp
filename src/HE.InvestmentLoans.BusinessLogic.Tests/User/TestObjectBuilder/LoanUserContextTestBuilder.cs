using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using Moq;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;

public class LoanUserContextTestBuilder
{
    private readonly Mock<ILoanUserContext> _mock;

    private LoanUserContextTestBuilder(UserAccount? userAccount)
    {
        _mock = new Mock<ILoanUserContext>();
        ReturnUserAccount(userAccount ?? UserAccountTestData.UserAccountOne);
    }

    public UserAccount UserAccountFromMock { get; private set; }

    public static LoanUserContextTestBuilder New(UserAccount? userAccount = null) => new(userAccount);

    public LoanUserContextTestBuilder ReturnUserAccount(UserAccount userAccount)
    {
        UserAccountFromMock = userAccount;
        _mock.Setup(x => x.GetSelectedAccount()).ReturnsAsync(userAccount);
        _mock.Setup(x => x.GetSelectedAccountId()).ReturnsAsync(userAccount.AccountId);
        return this;
    }

    public ILoanUserContext Build()
    {
        return _mock.Object;
    }

    public LoanUserContextTestBuilder Register(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return this;
    }
}
