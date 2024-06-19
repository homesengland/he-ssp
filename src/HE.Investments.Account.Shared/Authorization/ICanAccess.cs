using HE.Investments.Account.Api.Contract.User;

namespace HE.Investments.Account.Shared.Authorization;

public interface ICanAccess
{
    public Task<bool> CanAccess(IList<UserRole> allowedFor);
}
