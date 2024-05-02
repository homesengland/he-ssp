using HE.Investments.Account.Contract.Organisation;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Domain.Organisation.Entities;
using HE.Investments.Common.Contract;

namespace HE.Investments.Account.Domain.Organisation.Repositories;

public interface IOrganizationRepository
{
    public Task<OrganizationBasicInformation> GetBasicInformation(OrganisationId organisationId, CancellationToken cancellationToken);

    public Task<OrganisationChangeRequestState> GetOrganisationChangeRequestDetails(OrganisationId organisationId, CancellationToken cancellationToken);

    Task<OrganisationId> CreateOrganisation(OrganisationEntity organisation);

    Task Save(OrganisationId organisationId, OrganisationEntity organisation, CancellationToken cancellationToken);
}
