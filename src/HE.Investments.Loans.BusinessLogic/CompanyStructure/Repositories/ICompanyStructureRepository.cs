using HE.Investments.Account.Shared.User;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.Repositories;

public interface ICompanyStructureRepository
{
    Task<CompanyStructureEntity> GetAsync(LoanApplicationId loanApplicationId, UserAccount userAccount, CompanyStructureFieldsSet companyStructureFieldsSet, CancellationToken cancellationToken);

    Task<string> GetFilesLocationAsync(LoanApplicationId loanApplicationId, CancellationToken cancellationToken);

    Task SaveAsync(CompanyStructureEntity companyStructure, UserAccount userAccount, CancellationToken cancellationToken);
}
