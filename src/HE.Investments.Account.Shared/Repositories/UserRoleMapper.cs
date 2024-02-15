using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.Extensions;

namespace HE.Investments.Account.Shared.Repositories;

public static class UserRoleMapper
{
    private static readonly Dictionary<UserRole, invln_Permission> Roles = new()
    {
        { UserRole.Admin, invln_Permission.Admin },
        { UserRole.Enhanced, invln_Permission.Enhanced },
        { UserRole.Input, invln_Permission.Inputonly },
        { UserRole.ViewOnly, invln_Permission.Viewonly },
        { UserRole.Limited, invln_Permission.Limiteduser },
    };

    public static UserRole? ToDomain(int? value)
    {
        if (value == null)
        {
            return null;
        }

        var contract = (invln_Permission)value;
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
