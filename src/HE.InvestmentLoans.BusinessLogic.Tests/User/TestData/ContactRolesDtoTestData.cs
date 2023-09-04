extern alias Org;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.TestData;

public static class ContactRolesDtoTestData
{
    public static readonly ContactRolesDto ContactRolesDto = new()
    {
        contactRoles = new List<ContactRoleDto>() {
            new()
            {
                accountId = Guid.NewGuid(),
                accountName = "Organization",
                webRoleName = "Director",
                permissionLevel = "Test permission leve",
            },
        },
        email = "test@test.com",
        externalId = "UserOne",
    };
}
