using HE.Investments.Account.Domain.Users.Entities;

namespace HE.Investments.Account.Domain.Users.Repositories;

public interface IUserRepository
{
    Task<UserEntity> GetUser(string id, Guid organisationId, CancellationToken cancellationToken);

    Task Save(UserEntity entity, Guid organisationId, CancellationToken cancellationToken);
}
