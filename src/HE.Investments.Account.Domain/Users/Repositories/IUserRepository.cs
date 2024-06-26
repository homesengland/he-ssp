using HE.Investments.Account.Domain.Users.Entities;
using HE.Investments.Common.Contract;

namespace HE.Investments.Account.Domain.Users.Repositories;

public interface IUserRepository
{
    Task<UserEntity> GetUser(UserGlobalId userGlobalId, OrganisationId organisationId, CancellationToken cancellationToken);

    Task Save(UserEntity entity, UserGlobalId userAssigningId, OrganisationId organisationId, CancellationToken cancellationToken);
}
