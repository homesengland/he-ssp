using HE.Investments.Account.Shared.User;
using HE.Investments.Loans.BusinessLogic.Files;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.Repositories;

public interface ICompanyStructureRepository : ILoansFileLocationProvider<LoanApplicationId>
{
    Task<CompanyStructureEntity> GetAsync(LoanApplicationId loanApplicationId, UserAccount userAccount, CompanyStructureFieldsSet companyStructureFieldsSet, CancellationToken cancellationToken);

    Task SaveAsync(CompanyStructureEntity companyStructure, UserAccount userAccount, CancellationToken cancellationToken);
}
