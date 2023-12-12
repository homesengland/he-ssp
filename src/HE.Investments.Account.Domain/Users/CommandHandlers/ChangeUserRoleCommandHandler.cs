using HE.Investments.Account.Contract.Users.Commands;
using HE.Investments.Account.Domain.Users.Entities;
using HE.Investments.Account.Domain.Users.Repositories;

namespace HE.Investments.Account.Domain.Users.CommandHandlers;

public class ChangeUserRoleCommandHandler : UpdateUserCommandHandler<ChangeUserRoleCommand>
{
    public ChangeUserRoleCommandHandler(IUserRepository repository)
        : base(repository)
    {
    }

    protected override void Update(UserEntity entity, ChangeUserRoleCommand request)
    {
        entity.ChangeRole(request.NewRole);
    }
}
