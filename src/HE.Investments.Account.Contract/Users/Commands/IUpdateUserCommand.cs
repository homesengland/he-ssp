using HE.Investments.Common.Contract;

namespace HE.Investments.Account.Contract.Users.Commands;

public interface IUpdateUserCommand
{
    public UserGlobalId UserId { get; }
}
