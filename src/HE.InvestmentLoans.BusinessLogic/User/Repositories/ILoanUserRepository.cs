using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Contract.User.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.User.Repositories;

public interface ILoanUserRepository
{
    public Task<ContactRolesDto?> GetUserAccount(UserGlobalId userGlobalId, string userEmail);

    public Task<UserDetails> GetUserDetails(UserGlobalId userGlobalId);

    public Task SaveAsync(UserDetails userDetails, UserGlobalId userGlobalId, CancellationToken cancellationToken);
}
