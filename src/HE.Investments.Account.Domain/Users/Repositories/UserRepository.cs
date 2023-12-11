using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Domain.Data;
using HE.Investments.Account.Domain.Users.Entities;
using HE.Investments.Account.Domain.Users.ValueObjects;

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
        var role = await _usersCrmContext.GetUserRole(id, user.email);

        // TODO #86130: change mapping after CRM changes
        UserRole? r = role switch
        {
            "Admin" => UserRole.Admin,
            "View Only" => UserRole.ViewOnly,
            "Enhanced" => UserRole.Enhanced,
            "Input Only" => UserRole.Input,
            _ => null,
        };

        var entity = new UserEntity(new UserId(user.contactExternalId), user.firstName, user.lastName, user.email, user.jobTitle, r, null);

        return entity;
    }

    public async Task Save(UserEntity entity, CancellationToken cancellationToken)
    {
        if (entity.IsRoleModified)
        {
            // TODO #86130: change mapping after CRM changes
            var role = entity.Role switch
            {
                UserRole.Admin => "Admin",
                UserRole.ViewOnly => "View Only",
                UserRole.Enhanced => "Enhanced",
                UserRole.Input => "Input Only",
                _ => null,
            };
            if (role != null)
            {
                await _usersCrmContext.ChangeUserRole(entity.Id.Value, role);
            }
        }
    }
}
