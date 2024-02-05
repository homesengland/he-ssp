using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Tests.TestData;

namespace HE.Investments.Account.Shared.Tests.User.TestData;

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
