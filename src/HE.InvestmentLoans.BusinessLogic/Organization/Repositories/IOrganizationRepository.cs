using HE.InvestmentLoans.BusinessLogic.Organization.Entities;
using HE.InvestmentLoans.BusinessLogic.Organization.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.Organization.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Organization.Repositories;

public interface IOrganizationRepository
{
    public Task<OrganizationBasicInformation> GetBasicInformation(UserAccount userAccount, CancellationToken cancellationToken);

    public Task<OrganisationChangeRequestState> GetOrganisationChangeRequestDetails(UserAccount userAccount, CancellationToken cancellationToken);

    Task<Guid> CreateOrganisation(OrganisationEntity organisation);

    Task Update(OrganisationEntity organisation, UserAccount userAccount, CancellationToken cancellationToken);
}
