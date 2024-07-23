using HE.Investments.Loans.BusinessLogic.Files;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure;

public record CompanyStructureFileParams(LoanApplicationId LoanApplicationId) : ILoansFileParams
{
    public string PartitionId => LoanApplicationId.Value.ToString();
}
