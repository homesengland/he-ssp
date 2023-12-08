using System.ComponentModel;

namespace HE.Investments.Account.Contract.Users;

public enum UserRole
{
    Undefined,
    Limited,
    [Description("View only")]
    ViewOnly,
    Input,
    Enhanced,
    Admin,
}
