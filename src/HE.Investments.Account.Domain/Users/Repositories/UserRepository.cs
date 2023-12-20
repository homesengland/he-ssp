using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Domain.Data;
using HE.Investments.Account.Domain.Users.Entities;
using HE.Investments.Account.Domain.Users.ValueObjects;
using HE.Investments.Account.Shared.Repositories;

namespace HE.Investments.Account.Domain.Users.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IUsersCrmContext _usersCrmContext;

    public UserRepository(IUsersCrmContext usersCrmContext)
    {
        _usersCrmContext = usersCrmContext;
    }

    public async Task<UserEntity> GetUser(string id, CancellationToken cancellationToken)
    {
        var user = await _usersCrmContext.GetUser(id);
        var role = await _usersCrmContext.GetUserRole(id);

        return new UserEntity(
            new UserId(user.contactExternalId),
            user.firstName,
            user.lastName,
            user.email,
            user.jobTitle,
            UserRoleMapper.ToDomain(role),
            null);
    }

    public async Task Save(UserEntity entity, CancellationToken cancellationToken)
    {
        if (entity.IsRoleModified)
        {
            var role = UserRoleMapper.ToDto(entity.Role);
            if (role != null)
            {
                await _usersCrmContext.ChangeUserRole(entity.Id.Value, role.Value);
            }
        }
    }
}
