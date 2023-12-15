using HE.Investments.Account.Domain.Organisation.ValueObjects;
using HE.Investments.Account.Domain.UserOrganisation.Entities;

namespace HE.Investments.Account.Domain.UserOrganisation.Repositories;

public interface IOrganisationUsersRepository
{
    Task<OrganisationUsersEntity> GetOrganisationUsers(OrganisationId organisationId, CancellationToken cancellationToken);

    Task Save(OrganisationUsersEntity organisationUsers, CancellationToken cancellationToken);
}
