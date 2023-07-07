using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.InvestmentLoans.BusinessLogic.User.Repositories;

public interface ILoanUserRepository
{
    public ContactRolesDto GetUserDetails(string userGlobalId, string userEmail);
}
