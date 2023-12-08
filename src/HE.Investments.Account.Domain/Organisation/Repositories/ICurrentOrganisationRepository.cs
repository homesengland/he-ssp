using HE.Investments.Account.Contract.Organisation.Queries;

namespace HE.Investments.Account.Domain.Organisation.Repositories;

public interface ICurrentOrganisationRepository
{
    Task<OrganizationBasicInformation> GetBasicInformation(CancellationToken cancellationToken);
}
