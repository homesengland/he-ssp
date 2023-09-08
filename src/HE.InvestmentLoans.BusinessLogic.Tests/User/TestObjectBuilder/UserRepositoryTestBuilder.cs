extern alias Org;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.BusinessLogic.User.Repositories;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.User.ValueObjects;
using Moq;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
public class UserRepositoryTestBuilder
{
    private readonly Mock<ILoanUserRepository> _mock;

    private UserRepositoryTestBuilder()
    {
        _mock = new Mock<ILoanUserRepository>();
    }

    public static UserRepositoryTestBuilder New() => new();

    public UserRepositoryTestBuilder ReturnUserAccountEntity(
        UserGlobalId userGlobalId,
        string userEmail,
        ContactRolesDto contactRolesDto)
    {
        _mock.Setup(x => x.GetUserRoles(userGlobalId, userEmail)).ReturnsAsync(contactRolesDto);
        return this;
    }

    public UserRepositoryTestBuilder ReturnUserDetailsEntity(
        UserGlobalId userGlobalId,
        UserDetails userDetails)
    {
        _mock.Setup(x => x.GetUserDetails(userGlobalId)).ReturnsAsync(userDetails);
        return this;
    }

    public ILoanUserRepository Build()
    {
        return _mock.Object;
    }

    public Mock<ILoanUserRepository> BuildMockAndRegister(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return _mock;
    }
}
