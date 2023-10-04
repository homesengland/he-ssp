extern alias Org;
using HE.InvestmentLoans.BusinessLogic.Organization.Repositories;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Organization.TestObjectBuilder;
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
