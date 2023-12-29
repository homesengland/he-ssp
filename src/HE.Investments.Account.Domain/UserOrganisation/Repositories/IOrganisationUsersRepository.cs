using HE.Investments.Account.Domain.UserOrganisation.Entities;
using HE.Investments.Account.Shared.User.ValueObjects;

namespace HE.Investments.Account.Domain.UserOrganisation.Repositories;

public interface IOrganisationUsersRepository
{
    Task<OrganisationUsersEntity> GetOrganisationUsers(OrganisationId organisationId, CancellationToken cancellationToken);

    Task Save(OrganisationUsersEntity organisationUsers, CancellationToken cancellationToken);
}
