extern alias Org;
using HE.InvestmentLoans.Common.Tests.TestData;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.TestData;

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
            },
        },
        email = "test@test.com",
        externalId = "UserOne",
    };
}
