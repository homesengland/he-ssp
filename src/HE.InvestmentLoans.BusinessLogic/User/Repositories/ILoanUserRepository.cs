extern alias Org;

using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using HE.InvestmentLoans.Contract.User.ValueObjects;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.InvestmentLoans.BusinessLogic.User.Repositories;

public interface ILoanUserRepository
{
    public Task<IList<UserAccount>> GetUserAccounts(UserGlobalId userGlobalId, string userEmail);

    public Task<UserDetails> GetUserDetails(UserGlobalId userGlobalId);

    public Task SaveAsync(UserDetails userDetails, UserGlobalId userGlobalId, CancellationToken cancellationToken);
}
