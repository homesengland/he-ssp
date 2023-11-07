extern alias Org;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Domain.Organisation.Repositories;
using HE.Investments.Account.Shared.User;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investments.Account.Domain.Tests.Organisation.TestObjectBuilder;
public class OrganizationRepositoryTestBuilder
{
    private readonly Mock<IOrganizationRepository> _mock;

    private OrganizationRepositoryTestBuilder()
    {
        _mock = new Mock<IOrganizationRepository>();
    }

    public static OrganizationRepositoryTestBuilder New() => new();

    public OrganizationRepositoryTestBuilder ReturnOrganizationBasicInformationEntity(
        UserAccount userAccount,
        OrganizationBasicInformation organizationBasicInformation)
    {
        _mock.Setup(x => x.GetBasicInformation(userAccount, CancellationToken.None)).ReturnsAsync(organizationBasicInformation);
        return this;
    }

    public IOrganizationRepository Build()
    {
        return _mock.Object;
    }

    public Mock<IOrganizationRepository> BuildMockAndRegister(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return _mock;
    }
}
