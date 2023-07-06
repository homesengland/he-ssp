using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.InvestmentLoans.BusinessLogic.Repositories;

public interface ILoanApplicationRepository
{
    void Save(LoanApplicationDto loanApplicationDto);
}
