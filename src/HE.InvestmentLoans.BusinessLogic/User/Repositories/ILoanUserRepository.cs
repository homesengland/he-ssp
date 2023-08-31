using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.Contract.User.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.User.Repositories;

public interface ILoanUserRepository
{
    public Task<ContactRolesDto?> GetUserAccount(UserGlobalId userGlobalId, string userEmail);
}
