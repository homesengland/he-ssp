extern alias Org;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Tests.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestData;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using Microsoft.PowerPlatform.Dataverse.Client;
using Moq;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;
using Org::HE.Investments.Organisation.Services;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
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
            contactRolesDto.externalId)).ReturnsAsync(contactRolesDto);

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
