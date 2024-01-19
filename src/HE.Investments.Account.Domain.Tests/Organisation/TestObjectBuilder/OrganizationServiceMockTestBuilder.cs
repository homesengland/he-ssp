extern alias Org;

using HE.Investments.Account.Domain.Organisation.Entities;
using HE.Investments.Account.Domain.Tests.Organisation.TestData;
using HE.Investments.Account.Domain.Tests.User.TestData;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Tests.TestData;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;
using Org::HE.Investments.Organisation.Services;

namespace HE.Investments.Account.Domain.Tests.Organisation.TestObjectBuilder;

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
                UserAccountFromMock.SelectedOrganisationId().ToString(),
                UserAccountFromMock.UserGlobalId.ToString()))
            .ReturnsAsync(OrganizationDetailsDtoMock);

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
