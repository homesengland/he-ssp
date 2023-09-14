using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestData;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using Moq;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;

public class LoanUserContextTestBuilder
{
    private readonly Mock<ILoanUserContext> _mock;

    private LoanUserContextTestBuilder(UserAccount? userAccount, UserDetails? userDetails = null)
    {
        _mock = new Mock<ILoanUserContext>();
        ReturnUserAccount(userAccount ?? UserAccountTestData.UserAccountOne);
        ReturnUserDetails(userDetails ?? UserDetailsTestData.UserDetailsOne);
    }

    public UserAccount UserAccountFromMock { get; private set; }

    public UserDetails UserDetailsFromMock { get; private set; }

    public static LoanUserContextTestBuilder New(UserAccount? userAccount = null) => new(userAccount);

    public LoanUserContextTestBuilder ReturnUserAccount(UserAccount userAccount)
    {
        UserAccountFromMock = userAccount;
        _mock.Setup(x => x.GetSelectedAccount()).ReturnsAsync(userAccount);
        _mock.Setup(x => x.GetSelectedAccountId()).ReturnsAsync(userAccount.AccountId);
        _mock.Setup(x => x.GetAllAccounts()).ReturnsAsync(new List<UserAccount> { userAccount });
        _mock.Setup(x => x.UserGlobalId).Returns(userAccount.UserGlobalId);
        _mock.Setup(x => x.Email).Returns(userAccount.UserEmail);

        return this;
    }

    public LoanUserContextTestBuilder IsNotLinkedWithOrganization()
    {
        _mock.Setup(x => x.IsLinkedWithOrganization()).ReturnsAsync(false);

        return this;
    }

    public LoanUserContextTestBuilder IsLinkedWithOrganization()
    {
        _mock.Setup(x => x.IsLinkedWithOrganization()).ReturnsAsync(true);

        return this;
    }

    public LoanUserContextTestBuilder ReturnUserDetails(UserDetails userDetails)
    {
        UserDetailsFromMock = userDetails;
        _mock.Setup(x => x.GetUserDetails()).ReturnsAsync(userDetails);
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
