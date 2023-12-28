using HE.Investments.Account.Contract.Users.Commands;
using HE.Investments.Account.Domain.Users.Entities;
using HE.Investments.Account.Domain.Users.Repositories;
using HE.Investments.Account.Shared;

namespace HE.Investments.Account.Domain.Users.CommandHandlers;

public class ChangeUserRoleCommandHandler : UpdateUserCommandHandler<ChangeUserRoleCommand>
{
    public ChangeUserRoleCommandHandler(IUserRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override void Update(UserEntity entity, ChangeUserRoleCommand request)
    {
        entity.ChangeRole(request.NewRole);
    }
}
