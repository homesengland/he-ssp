using HE.Investment.AHP.Domain.Tests.Common.TestData;
using HE.Investment.AHP.Domain.UserContext;
using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Account.Shared.User.Entities;
using HE.Investments.Consortium.Shared.UserContext;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investment.AHP.Domain.Tests.Common.TestDataBuilders;

public class ConsortiumUserContextTestBuilder
{
    private readonly Mock<IConsortiumUserContext> _mock;

    private ConsortiumUserContextTestBuilder(ConsortiumUserAccount? userAccount)
    {
        _mock = new Mock<IConsortiumUserContext>();
        ReturnUserAccount(userAccount ?? AhpUserAccountTestData.UserAccountOneWithConsortium);
    }

    public ConsortiumUserAccount ConsortiumUserFromMock { get; private set; }

    public static ConsortiumUserContextTestBuilder New(ConsortiumUserAccount? userAccount = null) => new(userAccount);

    public ConsortiumUserContextTestBuilder ReturnUserAccount(ConsortiumUserAccount userAccount)
    {
        ConsortiumUserFromMock = userAccount;
        _mock.Setup(x => x.GetSelectedAccount()).ReturnsAsync(userAccount);
        _mock.Setup(x => x.UserGlobalId).Returns(userAccount.UserGlobalId);
        _mock.Setup(x => x.Email).Returns(userAccount.UserEmail);

        return this;
    }

    public ConsortiumUserContextTestBuilder IsNotLinkedWithOrganization()
    {
        _mock.Setup(x => x.IsLinkedWithOrganisation()).ReturnsAsync(false);

        return this;
    }

    public ConsortiumUserContextTestBuilder IsLinkedWithOrganization()
    {
        _mock.Setup(x => x.IsLinkedWithOrganisation()).ReturnsAsync(true);

        return this;
    }

    public IConsortiumUserContext Build()
    {
        return _mock.Object;
    }

    public ConsortiumUserContextTestBuilder Register(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return this;
    }

    public ConsortiumUserContextTestBuilder ReturnProfileDetails(UserProfileDetails profileDetails)
    {
        _mock.Setup(x => x.GetProfileDetails()).ReturnsAsync(profileDetails);

        return this;
    }
}
