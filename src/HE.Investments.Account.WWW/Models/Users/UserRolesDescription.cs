using HE.Investments.Account.Api.Contract;
using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Models;

namespace HE.Investments.Account.WWW.Models.Users;

public static class UserRolesDescription
{
    public static IDictionary<UserRole, string> All => new Dictionary<UserRole, string>
    {
        {
            UserRole.Admin,
            "Users will be able to create, view, edit and submit applications on behalf of the organisation. They will be able to edit all available information, add or remove users and change user roles. All admin users must be approved by Homes England."
        },
        {
            UserRole.Enhanced,
            "Users will be able to create, view, edit and submit applications on behalf of the organisation. They will be able to view all available information. They will not be able to add or remove users."
        },
        {
            UserRole.Input,
            "Users will be able to create, view, and edit applications on behalf of the organisation. They will be able to view all available information. They will not be able to submit applications, add or remove users."
        },
        {
            UserRole.ViewOnly,
            "Users will be able to view all applications and available information at the organisation. They will not be able to create, edit or submit any information."
        },
        { UserRole.Limited, "Users will be assigned this role when they are initially added. This is an interim role and cannot be selected." },
    };

    public static IEnumerable<ExtendedSelectListItem> ToAssign => All
        .Where(r => r.Key != UserRole.Limited)
        .ToDictionary(r => r.Key, r => r.Value)
        .ToExtendedSelectList();

    public static IEnumerable<ExtendedSelectListItem> ToInvite => All
        .Where(r => r.Key is not UserRole.Limited and not UserRole.Admin)
        .ToDictionary(r => r.Key, r => r.Value)
        .ToExtendedSelectList();
}
