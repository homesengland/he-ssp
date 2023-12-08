using HE.Investments.Account.Contract.Users;

namespace HE.Investments.Account.WWW.Models.Users;

public static class UserRoles
{
    private static readonly IDictionary<UserRole, string> Roles = new Dictionary<UserRole, string>
        {
            { UserRole.Limited, "Users will be assigned this role when they are initially added. This is an interim role and cannot be selected." },
            { UserRole.ViewOnly, "Users will be able to view all applications and available information at the organisation. They will not be able to create, edit or submit any information." },
            { UserRole.Input, "Users will be able to create, view, and edit applications on behalf of the organisation. They will be able to view all available information. They will not be able to submit applications, add or remove users." },
            { UserRole.Enhanced, "Users will be able to create, view, edit and submit applications on behalf of the organisation. They will be able to view all available information. They will not be able to add or remove users." },
            { UserRole.Admin, "Users will be able to create, view, edit and submit applications on behalf of the organisation. They will be able to edit all available information, add or remove users and change user roles. All admin users must be approved by Homes England." },
        };

    public static IDictionary<UserRole, string> GetAll() => Roles;
}
