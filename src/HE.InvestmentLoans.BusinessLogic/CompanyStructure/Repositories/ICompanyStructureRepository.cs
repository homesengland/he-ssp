using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;

public interface ICompanyStructureRepository
{
    Task<CompanyStructureEntity> GetAsync(LoanApplicationId loanApplicationId, UserAccount userAccount, CancellationToken cancellationToken);

    Task SaveAsync(CompanyStructureEntity companyStructure, UserAccount userAccount, CancellationToken cancellationToken);
}
