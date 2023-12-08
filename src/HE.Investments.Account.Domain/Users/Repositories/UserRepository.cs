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

    public async Task<UserEntity> GetUser(string id)
    {
        var user = await _usersCrmContext.GetUser(id);

        var entity = new UserEntity(new UserId(user.contactExternalId), user.firstName, user.lastName, user.email, user.jobTitle, null, null);

        return entity;
    }
}
