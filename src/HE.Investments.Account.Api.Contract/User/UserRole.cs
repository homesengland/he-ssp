using System.ComponentModel;

namespace HE.Investments.Account.Api.Contract.User;

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
