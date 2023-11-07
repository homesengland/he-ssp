using HE.InvestmentLoans.BusinessLogic.Organization.Entities;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Domain.Organisation.Entities;

namespace HE.Investments.Account.Domain.Organisation.Repositories;

public interface IOrganizationRepository
{
    public Task<OrganizationBasicInformation> GetBasicInformation(UserAccount userAccount, CancellationToken cancellationToken);

    public Task<OrganisationChangeRequestState> GetOrganisationChangeRequestDetails(UserAccount userAccount, CancellationToken cancellationToken);

    Task<Guid> CreateOrganisation(OrganisationEntity organisation);

    Task Update(OrganisationEntity organisation, UserAccount userAccount, CancellationToken cancellationToken);
}
