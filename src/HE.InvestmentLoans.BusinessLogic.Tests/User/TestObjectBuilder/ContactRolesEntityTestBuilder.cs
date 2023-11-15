extern alias Org;

using HE.Investments.Account.Domain.Tests.User.TestData;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
public class ContactRolesEntityTestBuilder
{
    private readonly ContactRolesDto _item;

    private ContactRolesEntityTestBuilder(ContactRolesDto contactRolesDto)
    {
        _item = contactRolesDto;
    }

    public static ContactRolesEntityTestBuilder New() =>
        new(new ContactRolesDto()
        {
            email = ContactRolesDtoTestData.ContactRolesDto.email,
            externalId = ContactRolesDtoTestData.ContactRolesDto.externalId,
            contactRoles = ContactRolesDtoTestData.ContactRolesDto.contactRoles,
        });

    public ContactRolesDto Build()
    {
        return _item;
    }
}
