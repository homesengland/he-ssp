extern alias Org;

using HE.Investments.Account.Domain.Tests.User.TestData;
using HE.Investments.TestsUtils.TestFramework;
using Microsoft.PowerPlatform.Dataverse.Client;
using Moq;
using ContactDto = Org::HE.Common.IntegrationModel.PortalIntegrationModel.ContactDto;
using ContactRolesDto = Org::HE.Common.IntegrationModel.PortalIntegrationModel.ContactRolesDto;
using IContactService = Org::HE.Investments.Organisation.Services.IContactService;

namespace HE.Investments.Account.Domain.Tests.User.Repositories.UserRepositoryTests;
public class ContactServiceMockTestBuilder
{
    private readonly Mock<IContactService> _mock;

    private ContactServiceMockTestBuilder(ContactDto? contactDto, ContactRolesDto? contactRolesDto = null)
    {
        _mock = new Mock<IContactService>();
        ReturnContactDto(contactDto ?? ContactDtoTestData.ContactDto);
        ReturnContactRolesDto(contactRolesDto ?? ContactRolesDtoTestData.ContactRolesDto);
    }

    public ContactDto ContactDtoFromMock { get; private set; }

    public ContactRolesDto ContactRolesFromMock { get; private set; }

    public static ContactServiceMockTestBuilder New(ContactDto? contactDto = null) => new(contactDto);

    public ContactServiceMockTestBuilder ReturnContactDto(ContactDto contactDto)
    {
        ContactDtoFromMock = contactDto;

        _mock.Setup(x => x.RetrieveUserProfile(It.IsAny<IOrganizationServiceAsync2>(), contactDto.contactId)).ReturnsAsync(contactDto);

        return this;
    }

    public ContactServiceMockTestBuilder ReturnContactRolesDto(ContactRolesDto contactRolesDto)
    {
        ContactRolesFromMock = contactRolesDto;

        _mock.Setup(x => x.GetContactRoles(
           It.IsAny<IOrganizationServiceAsync2>(),
           contactRolesDto.email,
           It.IsAny<string>(),
           It.IsAny<int>())).ReturnsAsync(contactRolesDto);
        return this;
    }

    public IContactService Build()
    {
        return _mock.Object;
    }

    public ContactServiceMockTestBuilder Register(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);
        return this;
    }

    public Mock<IContactService> BuildMock()
    {
        return _mock;
    }
}
