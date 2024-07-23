using HE.Investments.Loans.BusinessLogic.Files;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.ValueObjects;

public record SupportingDocumentsParams(LoanApplicationId LoanApplicationId) : ILoansFileParams
{
    public string PartitionId => LoanApplicationId.Value.ToString();

    public static SupportingDocumentsParams New(LoanApplicationId loanApplicationId) => new(loanApplicationId);
}
