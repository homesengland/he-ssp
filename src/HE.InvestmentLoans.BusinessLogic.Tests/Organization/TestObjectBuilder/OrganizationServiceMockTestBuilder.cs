extern alias Org;

using HE.InvestmentLoans.BusinessLogic.Organization.Entities;
using HE.InvestmentLoans.BusinessLogic.Tests.Organization.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestData;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;
using Org::HE.Investments.Organisation.Services;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Organization.TestObjectBuilder;

public class OrganizationServiceMockTestBuilder
{
    private readonly Mock<IOrganizationService> _mock;

    private OrganizationServiceMockTestBuilder(OrganizationDetailsDto? organizationDetailsDto, UserAccount? userAccount, OrganisationEntity? organisationEntity)
    {
        _mock = new Mock<IOrganizationService>();
        ReturnUserAccount(userAccount ?? UserAccountTestData.UserAccountOne);
        ReturnOrganizationDetailsDto(organizationDetailsDto ?? OrganizationDetailsDtoTestData.OrganizationDetailsDto);
        ReturnOrganizationEntity(organisationEntity ?? OrganisationEntityTestData.OrganisationEntity);
    }

    public UserAccount UserAccountFromMock { get; private set; }

    public OrganizationDetailsDto OrganizationDetailsDtoMock { get; private set; }

    public OrganisationEntity OrganizationEntityMock { get; private set; }

    public static OrganizationServiceMockTestBuilder New(
        OrganizationDetailsDto? organizationDetailsDto = null,
        UserAccount? userAccount = null,
        OrganisationEntity? organisationEntity = null)
        => new(organizationDetailsDto, userAccount, organisationEntity);

    public OrganizationServiceMockTestBuilder ReturnUserAccount(UserAccount userAccount)
    {
        UserAccountFromMock = userAccount;
        return this;
    }

    public OrganizationServiceMockTestBuilder ReturnOrganizationDetailsDto(OrganizationDetailsDto organizationDetailsDto)
    {
        OrganizationDetailsDtoMock = organizationDetailsDto;

        _mock.Setup(x => x.GetOrganizationDetails(
                UserAccountFromMock.AccountId.ToString()!,
                UserAccountFromMock.UserGlobalId.ToString()))
            .ReturnsAsync(organizationDetailsDto);

        return this;
    }

    public OrganizationServiceMockTestBuilder ReturnOrganizationEntity(OrganisationEntity organisationEntity)
    {
        OrganizationEntityMock = organisationEntity;
        return this;
    }

    public IOrganizationService Build()
    {
        return _mock.Object;
    }

    public OrganizationServiceMockTestBuilder Register(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return this;
    }

    public Mock<IOrganizationService> BuildMock()
    {
        return _mock;
    }
}
