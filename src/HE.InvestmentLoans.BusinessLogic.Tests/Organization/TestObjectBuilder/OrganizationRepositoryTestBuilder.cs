extern alias Org;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.Organization.Repositories;
using HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.BusinessLogic.User.Repositories;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using HE.InvestmentLoans.Contract.User.ValueObjects;
using Moq;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;

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
