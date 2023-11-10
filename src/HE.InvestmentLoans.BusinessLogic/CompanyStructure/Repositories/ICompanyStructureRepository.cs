using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Account.Shared.User;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;

public interface ICompanyStructureRepository
{
    Task<CompanyStructureEntity> GetAsync(LoanApplicationId loanApplicationId, UserAccount userAccount, CompanyStructureFieldsSet companyStructureFieldsSet, CancellationToken cancellationToken);

    Task<string> GetFilesLocationAsync(LoanApplicationId loanApplicationId, CancellationToken cancellationToken);

    Task SaveAsync(CompanyStructureEntity companyStructure, UserAccount userAccount, CancellationToken cancellationToken);
}
