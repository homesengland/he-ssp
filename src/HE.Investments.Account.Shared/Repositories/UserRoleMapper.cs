using HE.Investments.Account.Contract.Users;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.Extensions;

namespace HE.Investments.Account.Shared.Repositories;

public static class UserRoleMapper
{
    private static readonly Dictionary<UserRole, invln_permission> Roles = new()
    {
        { UserRole.Admin, invln_permission.Admin },
        { UserRole.Enhanced, invln_permission.Enhanced },
        { UserRole.Input, invln_permission.Inputonly },
        { UserRole.ViewOnly, invln_permission.Viewonly },
        { UserRole.Limited, invln_permission.Limiteduser },
    };

    public static UserRole? ToDomain(int? value)
    {
        if (value == null)
        {
            return null;
        }

        var contract = (invln_permission)value;
        if (!Roles.TryGetKeyByValue(contract, out var result))
        {
            throw new ArgumentException($"Not supported User Role value {value}");
        }

        return result;
    }

    public static int? ToDto(UserRole? value)
    {
        if (value == null)
        {
            return null;
        }

        if (!Roles.TryGetValue(value.Value, out var result))
        {
            throw new ArgumentException($"Not supported User Role value {value}");
        }

        return (int)result;
    }
}
