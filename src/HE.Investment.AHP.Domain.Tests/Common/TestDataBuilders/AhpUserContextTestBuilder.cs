using HE.Investment.AHP.Domain.Tests.Common.TestData;
using HE.Investment.AHP.Domain.UserContext;
using HE.Investments.Account.Shared.User.Entities;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investment.AHP.Domain.Tests.Common.TestDataBuilders;

public class AhpUserContextTestBuilder
{
    private readonly Mock<IAhpUserContext> _mock;

    private AhpUserContextTestBuilder(AhpUserAccount? userAccount)
    {
        _mock = new Mock<IAhpUserContext>();
        ReturnUserAccount(userAccount ?? AhpUserAccountTestData.UserAccountOneWithConsortium);
    }

    public AhpUserAccount AhpUserFromMock { get; private set; }

    public static AhpUserContextTestBuilder New(AhpUserAccount? userAccount = null) => new(userAccount);

    public AhpUserContextTestBuilder ReturnUserAccount(AhpUserAccount userAccount)
    {
        AhpUserFromMock = userAccount;
        _mock.Setup(x => x.GetSelectedAccount()).ReturnsAsync(userAccount);
        _mock.Setup(x => x.UserGlobalId).Returns(userAccount.UserGlobalId);
        _mock.Setup(x => x.Email).Returns(userAccount.UserEmail);

        return this;
    }

    public AhpUserContextTestBuilder IsNotLinkedWithOrganization()
    {
        _mock.Setup(x => x.IsLinkedWithOrganisation()).ReturnsAsync(false);

        return this;
    }

    public AhpUserContextTestBuilder IsLinkedWithOrganization()
    {
        _mock.Setup(x => x.IsLinkedWithOrganisation()).ReturnsAsync(true);

        return this;
    }

    public IAhpUserContext Build()
    {
        return _mock.Object;
    }

    public AhpUserContextTestBuilder Register(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return this;
    }

    public AhpUserContextTestBuilder ReturnProfileDetails(UserProfileDetails profileDetails)
    {
        _mock.Setup(x => x.GetProfileDetails()).ReturnsAsync(profileDetails);

        return this;
    }
}
