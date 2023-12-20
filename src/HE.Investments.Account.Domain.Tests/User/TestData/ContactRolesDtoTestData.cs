extern alias Org;

using HE.Investments.Common.CRM.Model;
using HE.Investments.Loans.Common.Tests.TestData;
using ContactRoleDto = Org::HE.Common.IntegrationModel.PortalIntegrationModel.ContactRoleDto;
using ContactRolesDto = Org::HE.Common.IntegrationModel.PortalIntegrationModel.ContactRolesDto;

namespace HE.Investments.Account.Domain.Tests.User.TestData;

public static class ContactRolesDtoTestData
{
    public static readonly ContactRolesDto ContactRolesDto = new()
    {
        contactRoles = new List<ContactRoleDto>()
        {
            new()
            {
                accountId = GuidTestData.GuidOne,
                accountName = "Organization",
                webRoleName = "Limited user",
                permissionLevel = "Test permission level",
                permission = 858110001,
            },
        },
        email = "test@test.com",
        externalId = "UserOne",
    };
}
