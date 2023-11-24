using HE.Investments.Account.Contract.Organisation;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Domain.Organisation.Entities;
using HE.Investments.Account.Shared.User;

namespace HE.Investments.Account.Domain.Organisation.Repositories;

public interface IOrganizationRepository
{
    public Task<OrganizationBasicInformation> GetBasicInformation(UserAccount userAccount, CancellationToken cancellationToken);

    public Task<OrganisationChangeRequestState> GetOrganisationChangeRequestDetails(UserAccount userAccount, CancellationToken cancellationToken);

    Task<Guid> CreateOrganisation(OrganisationEntity organisation);

    Task Save(OrganisationEntity organisation, UserAccount userAccount, CancellationToken cancellationToken);
}
