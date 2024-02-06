using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.Tests.User.TestData;
using HE.Investments.Organisation.Services;
using HE.Investments.TestsUtils.TestFramework;
using Microsoft.PowerPlatform.Dataverse.Client;
using Moq;

namespace HE.Investments.Account.Shared.Tests.Repositories.UserRepositoryTests;
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
           It.IsAny<int?>())).ReturnsAsync(contactRolesDto);
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
