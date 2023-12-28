using HE.Investments.Account.Domain.Users.Entities;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.ValueObjects;

namespace HE.Investments.Account.Domain.Users.Repositories;

public interface IUserRepository
{
    Task<UserEntity> GetUser(UserGlobalId userGlobalId, OrganisationId organisationId, CancellationToken cancellationToken);

    Task Save(UserEntity entity, OrganisationId organisationId, CancellationToken cancellationToken);
}
