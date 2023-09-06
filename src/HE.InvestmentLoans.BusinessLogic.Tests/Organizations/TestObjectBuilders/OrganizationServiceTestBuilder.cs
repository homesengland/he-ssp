extern alias Org;

using HE.InvestmentLoans.BusinessLogic.Organization.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.Contract.Exceptions;
using HE.InvestmentLoans.Contract.Organization;
using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using MediatR;
using Moq;
using Org.HE.Common.IntegrationModel.PortalIntegrationModel;
using Org.HE.Investments.Organisation.Contract;
using Org.HE.Investments.Organisation.Services;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Organizations.TestObjectBuilders;

public class OrganizationServiceTestBuilder
{
    private readonly Mock<IOrganizationService> _mock;

    public OrganizationServiceTestBuilder()
    {
        _mock = new Mock<IOrganizationService>();
    }

    public static OrganizationServiceTestBuilder New() => new();

    public OrganizationServiceTestBuilder Register(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);

        return this;
    }

    public IOrganizationService Build()
    {
        return _mock.Object;
    }

    public Mock<IOrganizationService> Mock() => _mock;
}
